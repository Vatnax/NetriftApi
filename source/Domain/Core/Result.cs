namespace Netrift.Domain.Core;

public readonly struct Result<TSuccess, TError>
{
  public TSuccess? Value { get; init; }
  public TError? Error { get; init; }
  public bool IsSuccess { get; init; }

  private Result(TSuccess value)
  {
    Value = value;
    Error = default;
    IsSuccess = true;
  }

  private Result(TError error)
  {
    Value = default;
    Error = error;
    IsSuccess = false;
  }

  public static Result<TSuccess, TError> Success(TSuccess value) => new Result<TSuccess, TError>(value);
  public static Result<TSuccess, TError> Failure(TError error) => new Result<TSuccess, TError>(error);
  public static implicit operator Result<TSuccess, TError>(TSuccess value) => new(value);
  public static implicit operator Result<TSuccess, TError>(TError error) => new(error);
  public T Match<T>(
    Func<TSuccess, T> value,
    Func<TError, T> error) =>
      IsSuccess ? value(Value!) : error(Error!);
}