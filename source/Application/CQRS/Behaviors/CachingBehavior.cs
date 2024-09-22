using System.Text;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Netrift.Domain.Constants;
using Netrift.Domain.Core;
using Newtonsoft.Json;

namespace Netrift.Application.CQRS.Behaviors;

/// <summary>
/// A CQRS behavior handling data caching.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public class CachingBehavior<TRequest, TResponse> :
  IPipelineBehavior<TRequest, TResponse>
  where TRequest : ICacheable
  where TResponse : Result
{

  private readonly IDistributedCache _cache;
  private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

  /// <summary>
  /// Constructs the behavior.
  /// </summary>
  /// <param name="cache">An object that handles data caching.</param>
  /// <param name="logger">An object that handles data logging.</param>
  public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
  {
    _cache = cache;
    _logger = logger;
  }

  /// <summary>
  /// Handles the caching logic.
  /// </summary>
  /// <param name="request">An object containing request information.</param>
  /// <param name="next">A delegate to the next function in the chain that has to be executed.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A <see cref="Task"/> with a <typeparamref name="TResponse"/> result object.</returns>
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    string? obj = await _cache.GetStringAsync(request.CacheKey);

    if (obj is not null)
    {
      // Cache hit!
      _logger.LogInformation("Cache HIT!");

      TResponse? response = JsonConvert.DeserializeObject<TResponse>(obj)!;
      return response;
    }

    // Cache miss!
    _logger.LogInformation("Cache MISS!");

    var result = await next();

    // Cache only if the operation was successful
    if (result.IsSuccess)
    {
      _logger.LogInformation("Data will be cached because the operation resulted in success.");

      DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(request.ExpirationTime + CacheConstants.DefaultCacheExpirationTime)
        .SetSlidingExpiration(request.ExpirationTime);
      await _cache.SetAsync(request.CacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)), options, cancellationToken);
    }
    else
    {
      _logger.LogInformation("Data will NOT be cached because the operation resulted in errors.");
    }


    return result;
  }
}
