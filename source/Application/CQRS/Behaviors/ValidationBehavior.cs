using Netrift.Domain.Enums;
using Netrift.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Behaviors;

/// <summary>
/// A CQRS behavior handling validation.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public class ValidationBehavior<TRequest, TResponse> :
  IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;
  private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

  /// <summary>
  /// Constructs the behavior.
  /// </summary>
  /// <param name="validators">A collection of objects that may handle the validation.</param>
  /// <param name="logger">An object that handles that logging.</param>
  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
  {
    _validators = validators;
    _logger = logger;
  }

  /// <summary>
  /// Handles the validation logic.
  /// </summary>
  /// <param name="request">An object containing request information.</param>
  /// <param name="next">A delegate to the next function in the chain that has to be executed.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A <see cref="Task"/> with a <typeparamref name="TResponse"/> result object.</returns>
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    // Checking if there are any validators
    if (!_validators.Any())
    {
      return await next();
    }

    // Validating the request data and selecting error messages
    var validationErrors = _validators
      .Select(v => v.Validate(request))
      .SelectMany(vr => vr.Errors)
      .Select(e => e.ErrorMessage)
      .Distinct();

    // Checking if there are any errors
    if (!validationErrors.Any())
    {
      return await next();
    }

    // If there are errors, return a result object
    try
    {
      return MakeResultObject(validationErrors);
    }
    catch (NonGenericResultTypeUsageException ex)
    {
      _logger.LogError(ex.Message);
      throw;
    }
  }

  /// <summary>
  /// Makes a result object.
  /// </summary>
  /// <param name="validationErrors">Validation errors which will be used to construct a result.</param>
  /// <returns>A result object.</returns>
  /// <exception cref="NonGenericResultTypeUsageException"></exception>
  private TResponse MakeResultObject(IEnumerable<string> validationErrors)
  {
    // Checking if the response type is a non-generic Result type. If so, throw an exception
    if (typeof(TResponse) == typeof(Result))
    {
      throw new NonGenericResultTypeUsageException();
    }

    // Some reflection magic
    object? failureResult = typeof(Result<>)
      .GetGenericTypeDefinition()
      .MakeGenericType(typeof(TResponse).GetGenericArguments())
      .GetMethod(nameof(Result<Result>.Failure)) // Result<Result> is used only to get the name of a method
      !.Invoke(null, [validationErrors, ErrorType.Failure]);

    return (failureResult as TResponse)!;
  }
}