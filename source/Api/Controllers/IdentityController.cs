using Netrift.Application.CQRS.Commands.CreateUserCommand;
using Application.DataTransferObjects.RequestDataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Netrift.Application.CQRS.Queries.GetUserByIdQuery;
using Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;
using Microsoft.AspNetCore.Authorization;
using Netrift.Application.CQRS.Queries.SignInQuery;
using Domain.Records;
using Netrift.Application.CQRS.Queries.SignOutQuery;

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

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var result = await _sender.Send(new GetUserByIdQuery() { UserId = id });

    return result.Match<IActionResult>(Ok, BadRequest, NotFound);
  }

  [HttpGet("{emailOrName}")]
  public async Task<IActionResult> GetByEmailOrName(string emailOrName)
  {
    var result = await _sender.Send(new GetUserByEmailOrNameQuery() { EmailOrName = emailOrName });
    return result.Match<IActionResult>(Ok, BadRequest, NotFound);
  }

  [Authorize]
  [HttpPost("create")]
  public async Task<IActionResult> Create(AppUserRequestDto appUserDto)
  {
    var result = await _sender.Send(new CreateUserCommand { AppUser = appUserDto });

    return result.Match<IActionResult>(value => CreatedAtAction(nameof(Create), value), BadRequest, NotFound);
  }

  [HttpPost("sign-in")]
  public async Task<IActionResult> SignIn(UserCredentials credentials)
  {
    var result = await _sender.Send(
      new SignInQuery() { Credentials = credentials });
    return result.Match<IActionResult>(value => Ok(value), BadRequest, NotFound);
  }

  [HttpPost("sign-out")]
  public new async Task<IActionResult> SignOut()
  {
    await _sender.Send(new SignOutQuery());
    return Ok();
  }
}