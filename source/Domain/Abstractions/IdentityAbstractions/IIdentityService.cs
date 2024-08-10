using Netrift.Domain.Core;

namespace Netrift.Domain.Abstractions.IdentityAbstractions;

public interface IIdentityService
{
  public Task<Result<Guid, IEnumerable<string>>> CreateUser(string userName, string email, string password);
}