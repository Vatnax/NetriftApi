using Moq;
using Netrift.Application.CQRS.Queries.SignOutQuery;
using Netrift.Domain.Abstractions.IdentityAbstractions;

namespace Netrift.Tests.CQRSTests.QueriesHandlersTests;

public class SignOutQueryHandlerTests
{
  private readonly Mock<IIdentityService> _identityServiceMock;

  public SignOutQueryHandlerTests()
  {
    _identityServiceMock = new();
  }

  [Fact]
  public async Task Handle_ShouldCallSignOut_Once()
  {
    // Arrange

    SignOutQueryHandler handler = new(_identityServiceMock.Object);
    SignOutQuery query = new SignOutQuery();

    // Act

    await handler.Handle(query, CancellationToken.None);

    // Assert

    _identityServiceMock.Verify(ism => ism.SignOutAsync(), Times.Once);
  }
}