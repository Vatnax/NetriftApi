namespace Netrift.Application.CQRS;

public interface ICacheable
{
  public string CacheKey { get; }
  public TimeSpan ExpirationTime { get; }
}