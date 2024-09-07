namespace Netrift.Domain.Records;

/// <summary>
/// Represents user's data passed in requests.
/// </summary>
/// <param name="UserName">User's name.</param>
/// <param name="Email">User' e-mail address.</param>
/// <param name="Password">User's password.</param>
public record UserRequestData(string UserName, string Email, string Password);

/// <summary>
/// Represents user's data passed in responses.
/// </summary>
/// <param name="Id">User's id.</param>
/// <param name="UserName">User's name.</param>
/// <param name="Email">Users' e-mail address.</param>
public record UserResponseData(Guid Id, string UserName, string Email);

/// <summary>
/// Represents user's credentials.
/// </summary>
/// <param name="EmailOrName">User's e-mail or username.</param>
/// <param name="Password">User's password.</param>
public record UserCredentials(string EmailOrName, string Password);