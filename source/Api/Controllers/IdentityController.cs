using Application.CQRS.Commands.CreateUserCommand;
using Application.DataTransferObjects.RequestDataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Netrift.Api.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
  private readonly ISender _sender;

  public IdentityController(ISender sender)
  {
    _sender = sender;
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create(AppUserRequestDto appUserDto)
  {
    var result = await _sender.Send(new CreateUserCommand { AppUser = appUserDto });

    return result.Match<IActionResult>(value => Ok(value), BadRequest);
  }
}