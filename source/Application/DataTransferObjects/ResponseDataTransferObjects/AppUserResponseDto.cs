namespace Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;

/// <summary>
/// User data transfer object for responses.
/// </summary>
public sealed class AppUserResponseDto
{
  public Guid Id { get; set; } = default;
  public string UserName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
}