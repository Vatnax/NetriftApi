namespace Domain.Records;

public record UserRequestData(string UserName, string Email, string Password);
public record UserResponseData(Guid Id, string UserName, string Email);
public record UserCredentials(string EmailOrName, string Password);