using Microsoft.AspNetCore.Identity;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Infrastructure.Identity.IdentityEntities;
using Domain.Records;

namespace Netrift.Infrastructure.Identity;

public sealed class IdentityService : IIdentityService
{
  private readonly UserManager<AppUser> _userManager;
  private readonly SignInManager<AppUser> _signInManager;

  public IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
  {
    _userManager = userManager;
    _signInManager = signInManager;
  }

  public async Task<(Guid id, IEnumerable<string> errors)> CreateUser(UserRequestData userData)
  {
    AppUser user = new()
    {
      UserName = userData.UserName,
      Email = userData.Email,
    };

    IdentityResult result = await _userManager.CreateAsync(user, userData.Password);

    return (Guid.Parse(user.Id), result.Errors.Select(err => err.Description));
  }

  public async Task<UserResponseData?> GetUserByEmailOrName(string emailOrName)
  {
    AppUser? user =
      await _userManager.FindByEmailAsync(emailOrName) ??
      await _userManager.FindByNameAsync(emailOrName);

    if (user is null)
    {
      return null;
    }

    return new UserResponseData(Guid.Parse(user.Id), user.UserName!, user.Email!);
  }

  public async Task<UserResponseData?> GetUserById(Guid id)
  {
    AppUser? user = await _userManager.FindByIdAsync(id.ToString());

    if (user is null)
    {
      return null;
    }

    return new UserResponseData(id, user.UserName!, user.Email!);
  }

  public async Task<bool> SignIn(UserCredentials credentials)
  {
    AppUser? user =
      await _userManager.FindByEmailAsync(credentials.EmailOrName) ??
      await _userManager.FindByNameAsync(credentials.EmailOrName);

    if (user is null)
    {
      return false;
    }

    var result = await _signInManager.PasswordSignInAsync(user, credentials.Password, true, false);
    return result.Succeeded;
  }

  public async Task SignOut()
  {
    await _signInManager.SignOutAsync();
  }
}