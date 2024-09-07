using FluentAssertions;
using FluentValidation.TestHelper;
using Netrift.Application.CQRS.Commands.CreateUserCommand;
using Netrift.Application.Validation;

namespace Netrift.Tests.ValidatorsTests;

public class CreateUserCommandValidatorTests
{
  private readonly CreateUserCommandValidator _validator;

  public CreateUserCommandValidatorTests()
  {
    _validator = new();
  }

  [Theory]
  [InlineData("Username", "email@email.com", "Password", true)]
  [InlineData("su", "email@email.com", "Password", false)]
  [InlineData("Very Long Username That is toooooo long", "email@email.com", "Password", false)]
  [InlineData("Username", "email", "", false)]
  [InlineData("Very Long Username That is toooooo long", "email that is not an email", "password", false)]
  [InlineData("", "", "", false)]
  public void Validator_ShouldResultInErrors_IfDataProvidedIsIncorrect(
    string username, string email, string password, bool expectedResult)
  {
    // Arrange

    CreateUserCommand command = new()
    {
      AppUser = new()
      {
        UserName = username,
        Email = email,
        Password = password
      }
    };

    // Act

    var result = _validator.TestValidate(command);

    // Assert

    result.IsValid.Should().Be(expectedResult);
  }

  [Fact]
  public void Validator_ShouldResultInErrors_IfUserIsNull()
  {
    // Arrange

    CreateUserCommand command = new() { AppUser = null! };

    // Act

    var result = _validator.TestValidate(command);

    // Arrange

    result.ShouldHaveValidationErrorFor(c => c.AppUser);
  }
}