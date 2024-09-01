using System.Reflection.Metadata.Ecma335;
using Domain.Enums;

namespace Netrift.Domain.Core;

public abstract class Result
{
  public bool IsSuccess { get; init; }
  public List<string>? Error { get; init; }
  public ErrorType ErrorType { get; init; }

  protected Result()
  {
    Error = default;
    IsSuccess = true;
    ErrorType = ErrorType.None;
  }

  protected Result(List<string> error, ErrorType errorType = ErrorType.Failure)
  {
    Error = error;
    IsSuccess = false;
    ErrorType = errorType;
  }
}

public sealed class Result<TSuccess> : Result
{
  public TSuccess? Value { get; init; }

  private Result(TSuccess value)
  {
    Value = value;
  }

  private Result(List<string> error, ErrorType errorType = ErrorType.Failure) : base(error, errorType)
  {
    Value = default;
  }

  public static Result<TSuccess> Success(TSuccess value) => new Result<TSuccess>(value);
  public static Result<TSuccess> Failure(List<string> error, ErrorType errorType = ErrorType.Failure) => new Result<TSuccess>(error, errorType);

  public T Match<T>(
    Func<TSuccess, T> value,
    Func<IEnumerable<string>, T> error,
    Func<IEnumerable<string>, T> notFound) =>
      ErrorType switch
      {
        ErrorType.None => value(this.Value!),
        ErrorType.Failure => error(this.Error!),
        ErrorType.NotFound => notFound(this.Error!),
        _ => throw new ArgumentOutOfRangeException()
      };
}