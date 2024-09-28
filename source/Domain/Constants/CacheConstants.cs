namespace Netrift.Domain.Constants;

/// <summary>
/// Set of constants for handling caching.
/// </summary>
public static class CacheConstants
{
  public static string UserCachePrefix = "user";
  public static TimeSpan DefaultCacheExpirationTime = TimeSpan.FromMinutes(2);
}