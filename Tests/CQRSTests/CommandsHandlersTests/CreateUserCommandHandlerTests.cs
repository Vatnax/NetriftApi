using Netrift.Domain.Enums;
using Netrift.Domain.Records;
using FluentAssertions;
using Moq;
using Netrift.Application.CQRS.Commands.CreateUserCommand;
using Netrift.Domain.Abstractions.IdentityAbstractions;

namespace Netrift.Tests.CQRSTests.CommandsHandlersTests;

public class CreateUserCommandHandlerTests
{
  private readonly Mock<IIdentityService> _identityServiceMock;

  public CreateUserCommandHandlerTests()
  {
    _identityServiceMock = new();
  }

  [Fact]
  public async Task Handle_ShouldReturnResultWithNewUsersId_WhenCreationSucceeds()
  {
    // Arrange

    CreateUserCommandHandler handler = new(_identityServiceMock.Object);
    _identityServiceMock.Setup(ism => ism.CreateUserAsync(It.IsAny<UserRequestData>()))
      .ReturnsAsync((Guid.NewGuid(), []));

    // Act

    var result = await handler.Handle(new CreateUserCommand()
    {
      AppUser = new()
      {
        Email = "mock@email.com",
        UserName = "mock",
        Password = "password"
      }
    }, CancellationToken.None);

    // Assert

    result.Value.Should().NotBeEmpty();
    result.Errors.Should().BeNull();
    result.ErrorType.Should().BeOneOf(ErrorType.None);
  }

  [Fact]
  public async Task Handle_ShouldReturnResultWithErrors_WhenCreationFails()
  {
    // Arrange

    CreateUserCommandHandler handler = new(_identityServiceMock.Object);
    _identityServiceMock.Setup(ism => ism.CreateUserAsync(It.IsAny<UserRequestData>()))
      .ReturnsAsync((Guid.Empty, ["Some errors"]));

    // Act

    var result = await handler.Handle(new CreateUserCommand()
    {
      AppUser = new()
      {
        Email = "mock@email.com",
        UserName = "mock",
        Password = "password"
      }
    }, CancellationToken.None);

    // Assert

    result.Value.Should().BeEmpty();
    result.Errors.Should().NotBeNull();
    result.ErrorType.Should().BeOneOf(ErrorType.Failure, ErrorType.NotFound);
  }
}