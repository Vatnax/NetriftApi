using Domain.Records;

namespace Netrift.Domain.Abstractions.IdentityAbstractions;

public interface IIdentityService
{
  public Task<UserResponseData?> GetUserById(Guid id);
  public Task<UserResponseData?> GetUserByEmailOrName(string emailOrName);
  public Task<(Guid id, IEnumerable<string> errors)> CreateUser(UserRequestData userData);
  public Task<bool> SignIn(UserCredentials credentials);
  public Task SignOut();
}