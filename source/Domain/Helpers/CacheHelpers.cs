namespace Netrift.Domain.Helpers;

/// <summary>
/// Set of helpers to handle caching.
/// </summary>
public static class CacheHelper
{
  /// <summary>
  /// Creates a cache key.
  /// </summary>
  /// <param name="prefix">Should indicate a category - e.g. user, tournament, group etc.</param>
  /// <param name="middlePart">Should indicate an id of the requested resource.</param>
  /// <param name="optionalPart">Should indicate other things that the key may need.</param>
  /// <returns>Cache key in format: prefix-middlePart-optionalPart or prefix-middlePart</returns>
  public static string CreateCacheKey(string prefix, string middlePart, string? optionalPart = null) =>
    optionalPart is null ? $"{prefix}-{middlePart}" : $"{prefix}-{middlePart}-{optionalPart}";
}