using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using AutoMapper;
using Netrift.Domain.Enums;
using Netrift.Domain.Records;
using FluentAssertions;
using Moq;
using Netrift.Application.CQRS.Queries.GetUserByIdQuery;
using Netrift.Domain.Abstractions.IdentityAbstractions;

namespace Netrift.Tests.CQRSTests.QueriesHandlersTests;

public class GetUserByIdQueryHandlerTests
{
  private readonly Mock<IIdentityService> _identityServiceMock;
  private readonly Mock<IMapper> _mapperMock;

  public GetUserByIdQueryHandlerTests()
  {
    _identityServiceMock = new();
    _mapperMock = new();
  }

  [Fact]
  public async Task Handle_ShouldReturnResultWithUserData_WhenUserWithGivenIdExists()
  {
    // Arrange

    GetUserByIdQueryHandler handler = new(_identityServiceMock.Object, _mapperMock.Object);
    Guid id = Guid.NewGuid();
    GetUserByIdQuery query = new() { UserId = id };
    UserResponseData userResponseData = new UserResponseData(id, "mock", "mock@email.com");

    _identityServiceMock.Setup(ism => ism.GetUserByIdAsync(id))
      .ReturnsAsync(userResponseData);

    _mapperMock.Setup(mm => mm.Map<UserResponseData, AppUserResponseDto>(userResponseData))
      .Returns(new AppUserResponseDto() { Email = userResponseData.Email, UserName = userResponseData.UserName, Id = userResponseData.Id });

    // Act

    var result = await handler.Handle(query, CancellationToken.None);

    // Assert

    result.Value.Should().NotBeNull();
    result.Value!.Id.Should().NotBeEmpty().And.Be(userResponseData.Id);
    result.Value.Email.Should().NotBeEmpty().And.BeEquivalentTo(userResponseData.Email);
    result.Value.UserName.Should().NotBeEmpty().And.BeEquivalentTo(userResponseData.UserName);
    result.Errors.Should().BeNull();
    result.ErrorType.Should().BeOneOf(ErrorType.None);
  }

  [Fact]
  public async Task Handle_ShouldReturnResultWithErrors_WhenUserWithGivenIdDoesNotExist()
  {
    // Arrange

    GetUserByIdQueryHandler handler = new(_identityServiceMock.Object, _mapperMock.Object);
    Guid id = Guid.NewGuid();
    GetUserByIdQuery query = new() { UserId = id };

    _identityServiceMock.Setup(ism => ism.GetUserByIdAsync(id))
      .ReturnsAsync((UserResponseData?)null);

    // Act

    var result = await handler.Handle(query, CancellationToken.None);

    // Assert

    result.Value.Should().BeNull();
    result.Errors.Should().NotBeNull();
    result.ErrorType.Should().BeOneOf(ErrorType.Failure, ErrorType.NotFound);
  }
}