using Netrift.Domain.Core;
using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Domain.Records;

namespace Netrift.Application.CQRS.Commands.CreateUserCommand;

public sealed class CreateUserCommandHandler :
  IRequestHandler<CreateUserCommand, Result<Guid>>
{
  private readonly IIdentityService _identityService;

  public CreateUserCommandHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  public async Task<Result<Guid>> Handle(
    CreateUserCommand request, CancellationToken cancellationToken)
  {
    var result =
      await _identityService.CreateUser(new UserRequestData(
        request.AppUser.UserName, request.AppUser.Email, request.AppUser.Password)
      );

    if (!result.errors.Any())
    {
      return Result<Guid>.Success(result.id);
    }

    return Result<Guid>.Failure(result.errors.ToList());
  }
}