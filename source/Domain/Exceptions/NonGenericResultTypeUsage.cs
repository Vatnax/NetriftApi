using Netrift.Domain.Core;

namespace Netrift.Domain.Exceptions;

/// <summary>
/// Represents an error when non-generic <see cref="Result"/> is used.
/// </summary>
[Serializable]
public class NonGenericResultTypeUsageException : Exception
{
  public NonGenericResultTypeUsageException() : base($"The non-generic {nameof(Result)} type usage has been detected. Use the generic version instead.") { }
  public NonGenericResultTypeUsageException(string message) : base(message) { }
  public NonGenericResultTypeUsageException(string message, Exception inner) : base(message, inner) { }
}