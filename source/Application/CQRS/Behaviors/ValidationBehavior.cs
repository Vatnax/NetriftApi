using Domain.Enums;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Netrift.Domain.Core;


namespace Netrift.Application.CQRS.Behaviors;

public class ValidationBehavior<TRequest, TResponse> :
  IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;
  private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
  {
    _validators = validators;
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (!_validators.Any())
    {
      return await next();
    }

    var validationErrors = _validators
      .Select(v => v.Validate(request))
      .SelectMany(vr => vr.Errors)
      .Select(e => e.ErrorMessage)
      .Distinct()
      .ToList();

    if (!validationErrors.Any())
    {
      return await next();
    }

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

  private TResponse MakeResultObject(List<string> validationErrors)
  {
    if (typeof(TResponse) == typeof(Result))
    {
      throw new NonGenericResultTypeUsageException();
    }

    object? failureResult = typeof(Result<>)
      .GetGenericTypeDefinition()
      .MakeGenericType(typeof(TResponse).GetGenericArguments())
      .GetMethod("Failure")
      !.Invoke(null, [validationErrors, ErrorType.Failure]);

    return (failureResult as TResponse)!;
  }
}