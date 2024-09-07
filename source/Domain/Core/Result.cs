using Netrift.Domain.Enums;

namespace Netrift.Domain.Core;

/// <summary>
/// An abstract, non-generic version of result wrapper. Used only to be inherited.
/// </summary>
public abstract class Result
{
  public bool IsSuccess => ErrorType is ErrorType.None;
  public IEnumerable<string>? Errors { get; init; }
  public ErrorType ErrorType { get; init; }

  /// <summary>
  /// Constructs the result (success).
  /// </summary>
  protected Result()
  {
    Errors = default;
    ErrorType = ErrorType.None;
  }

  /// <summary>
  /// Constructs the result (error).
  /// </summary>
  /// <param name="errors">Errors.</param>
  /// <param name="errorType">Error type.</param>
  protected Result(IEnumerable<string> errors, ErrorType errorType = ErrorType.Failure)
  {
    Errors = errors;
    ErrorType = errorType;
  }
}

/// <summary>
/// A generic version of result wrapper.
/// </summary>
/// <typeparam name="TSuccess"></typeparam>
public sealed class Result<TSuccess> : Result
{
  public TSuccess? Value { get; init; }

  /// <summary>
  /// Constructs a result (success).
  /// </summary>
  /// <param name="value">Value.</param>
  private Result(TSuccess value)
  {
    Value = value;
  }

  /// <summary>
  /// Constructs a result (error).
  /// </summary>
  /// <param name="errors">Errors.</param>
  /// <param name="errorType">Error type.</param>
  private Result(IEnumerable<string> errors, ErrorType errorType = ErrorType.Failure) : base(errors, errorType)
  {
    Value = default;
  }

  /// <summary>
  /// Constructs a result determining a success.
  /// </summary>
  /// <param name="value">Value.</param>
  /// <returns>A new result object with a value.</returns>
  public static Result<TSuccess> Success(TSuccess value) => new Result<TSuccess>(value);

  /// <summary>
  /// Constructs a result with a failure.
  /// </summary>
  /// <param name="errors">Errors.</param>
  /// <param name="errorType">Error type.</param>
  /// <returns>A new result object with determining failure with error messages.</returns>
  public static Result<TSuccess> Failure(IEnumerable<string> errors, ErrorType errorType = ErrorType.Failure) => new Result<TSuccess>(errors, errorType);

  /// <summary>
  /// Constructs a result with a failure but without any error messages.
  /// </summary>
  /// <param name="errorType">Error type.</param>
  /// <returns>A new result object determining a failure with empty collection of messages.</returns>
  public static Result<TSuccess> FailureWithoutErrorMessages(ErrorType errorType) => new Result<TSuccess>([], errorType);

  /// <summary>
  /// Matches a given delegate with current <see cref="ErrorType"/>.
  /// </summary>
  /// <typeparam name="T">Result type.</typeparam>
  /// <param name="value">A delegate that is used if the result is in the happy state.</param>
  /// <param name="error">A delegate that is used if the result is in the unhappy state.</param>
  /// <param name="notFound">A delegate that is use if the result is in the unhappy state and its <see cref="ErrorType"/> is <see cref="ErrorType.NotFound"/>.</param>
  /// <returns>A <typeparamref name="T"/> object.</returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public T Match<T>(
    Func<TSuccess, T> value,
    Func<IEnumerable<string>, T> error,
    Func<IEnumerable<string>, T> notFound) =>
      ErrorType switch
      {
        ErrorType.None => value(this.Value!),
        ErrorType.Failure => error(this.Errors!),
        ErrorType.NotFound => notFound(this.Errors!),
        _ => throw new ArgumentOutOfRangeException()
      };
}