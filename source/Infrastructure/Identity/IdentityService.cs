using Netrift.Domain.Core;
using Microsoft.AspNetCore.Identity;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Infrastructure.Identity.IdentityEntities;

namespace Netrift.Infrastructure.Identity;

public sealed class IdentityService : IIdentityService
{
  private readonly UserManager<AppUser> _userManager;

  public IdentityService(UserManager<AppUser> userManager)
  {
    _userManager = userManager;
  }

  public async Task<Result<Guid, IEnumerable<string>>> CreateUser(string userName, string email, string password)
  {
    AppUser user = new()
    {
      UserName = userName,
      Email = email,
    };

    IdentityResult result = await _userManager.CreateAsync(user, password);

    if (!result.Succeeded)
    {
      return result.Errors.Select(e => e.Description).ToList();
    }

    return Guid.Parse(user.Id);
  }
}