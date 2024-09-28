namespace Netrift.Application.DataTransferObjects.RequestDataTransferObjects;

/// <summary>
/// User data transfer object for requests.
/// </summary>
public sealed class AppUserRequestDto
{
  public string UserName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}