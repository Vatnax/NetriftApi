using FluentAssertions;
using FluentValidation.TestHelper;
using Netrift.Application.CQRS.Queries.SignInQuery;
using Netrift.Application.Validation;

namespace Netrift.Tests.ValidatorsTests;

public class SignInQueryValidatorTests
{
  private readonly SignInQueryValidator _validator;
  public SignInQueryValidatorTests()
  {
    _validator = new();
  }

  [Theory]
  [InlineData("Valid email or name", "Valid Password", true)]
  [InlineData("", "Valid Password", false)]
  [InlineData("Valid email or name", "", false)]
  [InlineData(" ", "\n", false)]
  [InlineData(null!, null!, false)]
  public void Validator_ShouldResultInErrors_IfEmailOrNameIsNullOrWhitespace(string emailOrName, string password, bool expectedResult)
  {
    // Arrange

    SignInQuery query = new()
    {
      Credentials = new(emailOrName, password)
    };

    // Act

    var result = _validator.TestValidate(query);

    // Assert

    result.IsValid.Should().Be(expectedResult);
  }

  [Fact]
  public void Validator_ShouldResultInErrors_IfCredentialsObjectIsNull()
  {
    // Arrange

    SignInQuery query = new() { Credentials = null! };

    // Act

    var result = _validator.TestValidate(query);

    // Arrange

    result.ShouldHaveValidationErrorFor(q => q.Credentials);
  }
}