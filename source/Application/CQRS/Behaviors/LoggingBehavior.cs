using MediatR;
using Microsoft.Extensions.Logging;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Behaviors;

/// <summary>
/// A CQRS behavior handling logging.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public class LoggingBehavior<TRequest, TResponse> :
  IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

  /// <summary>
  /// Constructs the behavior.
  /// </summary>
  /// <param name="logger">An object that handles data logging.</param>
  public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// Handles the logging logic.
  /// </summary>
  /// <param name="request">An object containing request information.</param>
  /// <param name="next">A delegate to the next function in the chain that has to be executed.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A <see cref="Task"/> with a <typeparamref name="TResponse"/> result object.</returns>
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    // Logging data with its name and identifier (hash code) - start.
    _logger.LogInformation("Starting processing {RequestName}#{RequestHash}...",
      typeof(TRequest).Name, request.GetHashCode());

    var result = await next();

    // Logging data with its name and identifier (hash code) - finish.
    _logger.LogInformation("Finished processing{RequestName}#{RequestHash}. Result: {RequestResult}",
      typeof(TRequest).Name, request.GetHashCode(), result.IsSuccess ? "Success" : "Failure");

    return result;
  }
}