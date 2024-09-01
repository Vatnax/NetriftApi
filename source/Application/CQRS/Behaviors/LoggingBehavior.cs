using MediatR;
using Microsoft.Extensions.Logging;
using Netrift.Domain.Core;


namespace Netrift.Application.CQRS.Behaviors;

public class LoggingBehavior<TRequest, TResponse> :
  IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

  public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  {
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting processing {RequestName}#{RequestHash}...",
      typeof(TRequest).Name, request.GetHashCode());

    var result = await next();

    _logger.LogInformation("Finished processing{RequestName}#{RequestHash}. Result: {RequestResult}",
      typeof(TRequest).Name, request.GetHashCode(), result.IsSuccess ? "Success" : "Failure");

    return result;
  }
}