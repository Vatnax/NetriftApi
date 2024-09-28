using Netrift.Domain.Enums;
using Netrift.Domain.Records;
using FluentAssertions;
using Moq;
using Netrift.Application.CQRS.Queries.SignInQuery;
using Netrift.Domain.Abstractions.IdentityAbstractions;

namespace Netrift.Tests.CQRSTests.QueriesHandlersTests;

public class SignInQueryHandlerTests
{

  private readonly Mock<IIdentityService> _identityServiceMock;

  public SignInQueryHandlerTests()
  {
    _identityServiceMock = new();
  }

  [Fact]
  public async Task Handle_ShouldReturnResultWithTrue_WhenSignInSucceeded()
  {
    // Arrange

    SignInQueryHandler handler = new(_identityServiceMock.Object);
    UserCredentials credentials = new("mock", "password");
    SignInQuery query = new SignInQuery() { Credentials = credentials };
    _identityServiceMock.Setup(ism => ism.SignInAsync(credentials)).ReturnsAsync(true);

    // Act
    var result =
      await handler.Handle(query, CancellationToken.None);

    // Assert

    result.Value.Should().BeTrue();
    result.Errors.Should().BeNull();
  }

  [Fact]
  public async Task Handle_ShouldReturnResultWithErrors_WhenSignInFailed()
  {
    // Arrange

    SignInQueryHandler handler = new(_identityServiceMock.Object);
    UserCredentials credentials = new("mock", "password");
    _identityServiceMock.Setup(ism => ism.SignInAsync(credentials)).ReturnsAsync(false);

    // Act
    var result =
      await handler.Handle(new SignInQuery() { Credentials = credentials }, CancellationToken.None);

    // Assert

    result.Value.Should().BeFalse();
    result.Errors.Should().NotBeNull();
    result.ErrorType.Should().BeOneOf(ErrorType.Failure, ErrorType.NotFound);
  }
}