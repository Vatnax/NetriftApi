using Netrift.Domain.Core;

namespace Domain.Exceptions;

[Serializable]
public class NonGenericResultTypeUsageException : Exception
{
  public NonGenericResultTypeUsageException() : base($"The non-generic {nameof(Result)} type usage has been detected. Use the generic version instead.") { }
  public NonGenericResultTypeUsageException(string message) : base(message) { }
  public NonGenericResultTypeUsageException(string message, Exception inner) : base(message, inner) { }
}