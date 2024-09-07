using Netrift.Domain.Records;

namespace Netrift.Domain.Abstractions.IdentityAbstractions;

/// <summary>
/// An interface for handling identity.
/// </summary>
public interface IIdentityService
{
  /// <summary>
  /// Gets user by id.
  /// </summary>
  /// <param name="id">The id of the requested user.</param>
  /// <returns>A <see cref="Task"/> with an object containing user's data or null if not found.</returns>
  public Task<UserResponseData?> GetUserByIdAsync(Guid id);

  /// <summary>
  /// Gets user by e-mail or username.
  /// </summary>
  /// <param name="emailOrName">The e-mail or username of the requested user.</param>
  /// <returns>A <see cref="Task"/> with an object conatining user's data or null if not found.</returns>
  public Task<UserResponseData?> GetUserByEmailOrNameAsync(string emailOrName);

  /// <summary>
  /// Creates a user.
  /// </summary>
  /// <param name="userData">User's data.</param>
  /// <returns>A <see cref="Task"/> with eiter a valid id if the creation succeeded or with errors otherwise.</returns>
  public Task<(Guid id, IEnumerable<string> errors)> CreateUserAsync(UserRequestData userData);

  /// <summary>
  /// Signs-in a user.
  /// </summary>
  /// <param name="credentials">User's credentials.</param>
  /// <returns>A <see cref="Task"/> with true if user has been signed-in correctly, false otherwise.</returns>
  public Task<bool> SignInAsync(UserCredentials credentials);

  /// <summary>
  /// Signs-out a user.
  /// </summary>
  /// <returns>A <see cref="Task"/> object determining that the sign-out succeeded.</returns>
  public Task SignOutAsync();
}