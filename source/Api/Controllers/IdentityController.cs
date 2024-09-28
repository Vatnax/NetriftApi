using Netrift.Application.CQRS.Commands.CreateUserCommand;
using Netrift.Application.DataTransferObjects.RequestDataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Netrift.Application.CQRS.Queries.GetUserByIdQuery;
using Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;
using Microsoft.AspNetCore.Authorization;
using Netrift.Application.CQRS.Queries.SignInQuery;
using Netrift.Domain.Records;
using Netrift.Application.CQRS.Queries.SignOutQuery;

namespace Netrift.Api.Controllers;

/// <summary>
/// A controller that handles the identity.
/// </summary>
[Route("/api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
  private readonly ISender _sender;

  /// <summary>
  /// Constructs the controller.
  /// </summary>
  /// <param name="sender">An object that handles CQRS pattern.</param>
  public IdentityController(ISender sender)
  {
    _sender = sender;
  }

  /// <summary>
  /// Handles an HTTP request for searching a user by its id.
  /// </summary>
  /// <param name="id">The id of the requested user.</param>
  /// <returns>A <see cref="Task"/> with <see cref="IActionResult"/> with a response depending on the result of the search.</returns>
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var result = await _sender.Send(new GetUserByIdQuery() { UserId = id });

    return result.Match<IActionResult>(Ok, BadRequest, NotFound);
  }

  /// <summary>
  /// Handles an HTTP request for searching a user by its e-mail or name.
  /// </summary>
  /// <param name="emailOrName">The email or name of the requested user.</param>
  /// <returns>A <see cref="Task"/> with <see cref="IActionResult"/> with a response depending on the result of the search.</returns>
  [HttpGet("{emailOrName}")]
  public async Task<IActionResult> GetByEmailOrName(string emailOrName)
  {
    var result = await _sender.Send(new GetUserByEmailOrNameQuery() { EmailOrName = emailOrName });
    return result.Match<IActionResult>(Ok, BadRequest, NotFound);
  }

  /// <summary>
  /// Handles an HTTP request for creating a user.
  /// </summary>
  /// <param name="appUserDto">User's data.</param>
  /// <returns>A <see cref="Task"/> with <see cref="IActionResult"/> with a response depending on the result of the creation.</returns>
  [Authorize]
  [HttpPost("create")]
  public async Task<IActionResult> Create(AppUserRequestDto appUserDto)
  {
    var result = await _sender.Send(new CreateUserCommand { AppUser = appUserDto });

    return result.Match<IActionResult>(value => CreatedAtAction(nameof(Create), value), BadRequest, NotFound);
  }

  /// <summary>
  /// Handles an HTTP request for signing-in a user.
  /// </summary>
  /// <param name="credentials">User's credentials</param>
  /// <returns>A <see cref="Task"/> with <see cref="IActionResult"/> with a response depending on the result of the sign-in.</returns>
  [HttpPost("sign-in")]
  public async Task<IActionResult> SignIn(UserCredentials credentials)
  {
    var result = await _sender.Send(
      new SignInQuery() { Credentials = credentials });
    return result.Match<IActionResult>(value => Ok(value), BadRequest, NotFound);
  }

  /// <summary>
  /// Handles an HTTP request for signing-out a user. It hides the default <see cref="ControllerBase.SignOut"/> method from <see cref="ControllerBase"/> class.
  /// </summary>
  /// <returns>A <see cref="Task"/> with <see cref="IActionResult"/> with an <see cref="ControllerBase.Ok"/> status.</returns>
  [HttpPost("sign-out")]
  public new async Task<IActionResult> SignOut()
  {
    await _sender.Send(new SignOutQuery());
    return Ok();
  }
}