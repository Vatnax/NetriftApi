using Netrift.Domain.Core;
using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Domain.Records;

namespace Netrift.Application.CQRS.Commands.CreateUserCommand;

/// <summary>
/// A CQRS command handler for creating a user.
/// </summary>
public sealed class CreateUserCommandHandler :
  IRequestHandler<CreateUserCommand, Result<Guid>>
{
  private readonly IIdentityService _identityService;

  /// <summary>
  /// Constructs the handler
  /// </summary>
  /// <param name="identityService">An object that handles the identity.</param>
  public CreateUserCommandHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  /// <summary>
  /// Handles the command for creating a user.
  /// </summary>
  /// <param name="request">An object containing request information.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A <see cref="Task"/> with a generic <see cref="Result"/> object with either failure or success depending on the creation result.</returns>
  public async Task<Result<Guid>> Handle(
    CreateUserCommand request, CancellationToken cancellationToken)
  {
    var result =
      await _identityService.CreateUserAsync(new UserRequestData(
        request.AppUser.UserName, request.AppUser.Email, request.AppUser.Password)
      );

    if (!result.errors.Any())
    {
      return Result<Guid>.Success(result.id);
    }

    return Result<Guid>.Failure(result.errors);
  }
}