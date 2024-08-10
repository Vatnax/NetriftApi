using Application.DataTransferObjects.ResponseDataTransferObjects;
using Netrift.Domain.Core;
using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;

namespace Application.CQRS.Commands.CreateUserCommand;

public sealed class CreateUserCommandHandler :
  IRequestHandler<CreateUserCommand, Result<Guid, IEnumerable<string>>>
{
  private readonly IIdentityService _identityService;

  public CreateUserCommandHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  public async Task<Result<Guid, IEnumerable<string>>> Handle(
    CreateUserCommand request, CancellationToken cancellationToken)
  {
    var result =
      await _identityService.CreateUser(request.AppUser.UserName, request.AppUser.Email, request.AppUser.Password);

    if (result.IsSuccess)
    {
      return result.Value;
    }

    return result.Error!.ToList();
  }
}