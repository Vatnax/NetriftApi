using FluentAssertions;
using Netrift.Domain.Core;
using Netrift.Domain.Enums;

namespace Netrift.Tests;

public class ResultTests
{
  [Theory]
  [InlineData("Failure")]
  [InlineData("NotFound")]
  [InlineData("Success")]
  public void Success_ShouldReturnResultObjectInHappyState_WhenCalled(string resultState)
  {
    // Arrange & Act

    Result<int> result = resultState switch
    {
      "Success" => Result<int>.Success(5),
      "Failure" => Result<int>.Failure(["error msg 1", "error msg 2"]),
      "NotFound" => Result<int>.FailureWithoutErrorMessages(ErrorType.NotFound),
      _ => throw new ArgumentOutOfRangeException()
    };

    // Assert

    switch (resultState)
    {
      case "Success":
        result.Value.Should().Be(5);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
        result.ErrorType.Should().Be(ErrorType.None);
        break;
      case "Failure":
        result.Value.Should().Be(default);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.ErrorType.Should().Be(ErrorType.Failure);
        break;
      case "NotFound":
        result.Value.Should().Be(default);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
        result.ErrorType.Should().Be(ErrorType.NotFound);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  [Theory]
  [InlineData("Failure")]
  [InlineData("NotFound")]
  [InlineData("Success")]
  public void Match_ShouldReturnResult_BasedOnErrorType(string resultState)
  {
    // Arrange

    Result<int> result = resultState switch
    {
      "Success" => Result<int>.Success(5),
      "Failure" => Result<int>.Failure(["error msg 1", "error msg 2"]),
      "NotFound" => Result<int>.FailureWithoutErrorMessages(ErrorType.NotFound),
      _ => throw new ArgumentOutOfRangeException()
    };

    Func<int, object> value = (int x) => x;
    Func<IEnumerable<string>, object> failure = (IEnumerable<string> errors) => errors;
    Func<IEnumerable<string>, object> notFound = (IEnumerable<string> errors) => errors;

    // Act

    var actResult = result.Match<object>(value, failure, notFound);

    // Assert

    switch (resultState)
    {
      case "Success":
        actResult.Should().Be(result.Value);
        break;
      case "Failure":
        actResult.Should().Be(result.Errors);
        break;
      case "NotFound":
        actResult.Should().Be(result.Errors);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}