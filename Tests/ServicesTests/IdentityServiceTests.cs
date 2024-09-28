using Netrift.Domain.Records;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Netrift.Infrastructure.Identity;
using Netrift.Infrastructure.Identity.IdentityEntities;

namespace Netrift.Tests.ServicesTests;

public class IdentityServiceTests
{
  private readonly Mock<UserManager<AppUser>> _mockUserManager;
  private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
  private const string CORRECT_PASSWORD = "Correct Password";
  private const string CORRECT_EMAIL_OR_NAME = "Correct Email Or Name";

  public IdentityServiceTests()
  {
    _mockUserManager = new Mock<UserManager<AppUser>>(
      new Mock<IUserStore<AppUser>>().Object,
      null!, null!, null!, null!, null!, null!, null!, null!);

    _mockSignInManager = new Mock<SignInManager<AppUser>>(
      _mockUserManager.Object,
      new Mock<IHttpContextAccessor>().Object,
      new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
      null!, null!, null!, null!
    );
  }

  [Fact]
  public async Task CreateUser_ShouldReturnNewUsersId_WhenCreationSucceeds()
  {
    // Arrange

    UserRequestData userData = new("mock", "mock@email.com", "password");
    _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), userData.Password))
            .ReturnsAsync(IdentityResult.Success);
    IdentityService identityService = new(_mockUserManager.Object, null!);

    // Act

    (Guid userId, IEnumerable<string> errors) = await identityService.CreateUserAsync(userData);

    // Assert

    userId.Should().NotBeEmpty();
    errors.Should().BeEmpty();
  }

  [Fact]
  public async Task CreateUser_ShouldReturnErrors_WhenCreationFails()
  {
    // Arrange

    var identityService = new IdentityService(_mockUserManager.Object, null!);
    var userData = new UserRequestData("mock", "mock@example.com", "password");

    var identityErrors = new List<IdentityError>
        {
            new IdentityError { Description = "Mock error message 1." },
            new IdentityError { Description = "Mock error message 2." }
        };

    _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), userData.Password))
        .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

    // Act

    var (userId, errors) = await identityService.CreateUserAsync(userData);

    // Assert

    userId.Should().BeEmpty();
    errors.Should().NotBeEmpty();
  }

  [Fact]
  public async Task GetUserById_ShouldReturnUserData_WhenUserWithGivenIdExists()
  {
    // Arrange

    var identityService = new IdentityService(_mockUserManager.Object, null!);
    Guid id = Guid.NewGuid();
    _mockUserManager.Setup(um => um.FindByIdAsync(id.ToString()))
      .ReturnsAsync(new AppUser() { Email = "mock@email.com", UserName = "mock" });

    // Act

    UserResponseData userData = (await identityService.GetUserByIdAsync(id))!;

    // Assert

    userData.Should().NotBeNull();
    userData.Email.Should().NotBeNullOrWhiteSpace();
    userData.UserName.Should().NotBeNullOrWhiteSpace();
    userData.Id.Should().NotBeEmpty();
  }

  [Fact]
  public async Task GetUserById_ShouldReturnNull_WhenUserWithGivenIdDoesNotExist()
  {
    // Arrange

    var identityService = new IdentityService(_mockUserManager.Object, null!);
    Guid id = Guid.NewGuid();
    _mockUserManager.Setup(um => um.FindByIdAsync(id.ToString()))
      .ReturnsAsync((AppUser?)null);

    // Act

    UserResponseData userData = (await identityService.GetUserByIdAsync(id))!;

    // Assert

    userData.Should().BeNull();
  }

  [Fact]
  public async Task GetUserByEmailOrName_ShouldReturnUserData_WhenUserWithGivenEmailOrNameExists()
  {
    // Arrange

    var identityService = new IdentityService(_mockUserManager.Object, null!);
    string emailOrName = "emailOrName";
    _mockUserManager.Setup(um => um.FindByNameAsync(emailOrName))
      .ReturnsAsync(new AppUser() { Email = "mock@email.com", UserName = "Mock" });
    _mockUserManager.Setup(um => um.FindByEmailAsync(emailOrName))
      .ReturnsAsync(new AppUser() { Email = "mock@email.com", UserName = "Mock" });

    // Act

    UserResponseData userData = (await identityService.GetUserByEmailOrNameAsync(emailOrName))!;

    // Assert

    userData.Should().NotBeNull();
    userData.Email.Should().NotBeNullOrWhiteSpace();
    userData.UserName.Should().NotBeNullOrWhiteSpace();
    userData.Id.Should().NotBeEmpty();
  }

  [Fact]
  public async Task GetUserByEmailOrName_ShouldReturnNull_WhenUserWithGivenEmailOrNameDoesNotExist()
  {
    // Arrange

    var identityService = new IdentityService(_mockUserManager.Object, null!);
    string emailOrName = "emailOrName";
    _mockUserManager.Setup(um => um.FindByNameAsync(emailOrName))
      .ReturnsAsync((AppUser?)null);
    _mockUserManager.Setup(um => um.FindByEmailAsync(emailOrName))
      .ReturnsAsync((AppUser?)null);

    // Act

    UserResponseData userData = (await identityService.GetUserByEmailOrNameAsync(emailOrName))!;

    // Assert

    userData.Should().BeNull();
  }

  [Theory]
  [InlineData("Correct Email Or Name", "Correct Password", true)] // Both credentials are correct
  [InlineData("Correct Email Or Name", "Incorrect Password", false)] // Only email or name is correct
  [InlineData("Incorrect Email Or Name", "Correct Password", false)] // Only password is correct
  [InlineData("Incorrect Email Or Name", "Incorrect Password", false)] // None of the credentials are correct
  public async Task SignIn_ShouldReturnTrueOrFalse_DependingOnSignInResult(
    string emailOrName, string password, bool expectedResult)
  {
    // Arrange

    bool credentialsCorrect = emailOrName == CORRECT_EMAIL_OR_NAME && password == CORRECT_PASSWORD;
    IdentityService identityService = new(_mockUserManager.Object, _mockSignInManager.Object);
    UserCredentials userCredentials = new(emailOrName, password);
    AppUser? appUser = credentialsCorrect ? new() : null;
    SignInResult signInResult = credentialsCorrect ? SignInResult.Success : SignInResult.Failed;

    _mockUserManager.Setup(um => um.FindByNameAsync(emailOrName))
      .ReturnsAsync(appUser);
    _mockUserManager.Setup(um => um.FindByEmailAsync(emailOrName))
      .ReturnsAsync(appUser);

    _mockSignInManager.Setup(
      sim => sim.PasswordSignInAsync(It.IsAny<AppUser>(), userCredentials.Password, true, false))
        .ReturnsAsync(signInResult);

    // Act

    bool result = await identityService.SignInAsync(userCredentials);

    // Assert

    result.Should().Be(expectedResult);
  }

  [Fact]
  public async Task SignOut_ShouldBeCalled_Once()
  {
    // Arrange

    IdentityService identityService = new(null!, _mockSignInManager.Object);

    // Act

    await identityService.SignOutAsync();

    // Assert

    _mockSignInManager.Verify(sim => sim.SignOutAsync(), Times.Once);
  }
}