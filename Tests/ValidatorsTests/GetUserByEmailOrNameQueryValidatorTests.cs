using FluentAssertions;
using FluentValidation.TestHelper;
using Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;
using Netrift.Application.Validation;

namespace Netrift.Tests.ValidatorsTests;

public class GetUserByEmailOrNameQueryValidatorTests
{
  private readonly GetUserByEmailOrNameQueryValidator _validator;
  public GetUserByEmailOrNameQueryValidatorTests()
  {
    _validator = new();
  }

  [Theory]
  [InlineData("Some email or username", true)]
  [InlineData("", false)]
  [InlineData(" ", false)]
  [InlineData(null, false)]
  public void Validator_ShouldResultInErrors_IfEmailOrNameIsNullOrWhitespace(string emailOrName, bool expectedResult)
  {
    // Arrange

    GetUserByEmailOrNameQuery query = new() { EmailOrName = emailOrName };

    // Act

    var result = _validator.TestValidate(query);

    // Assert

    result.IsValid.Should().Be(expectedResult);
  }
}