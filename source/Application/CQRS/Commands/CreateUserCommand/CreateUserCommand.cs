using Application.DataTransferObjects.RequestDataTransferObjects;
using Netrift.Domain.Core;
using MediatR;

namespace Netrift.Application.CQRS.Commands.CreateUserCommand;

public sealed class CreateUserCommand
  : IRequest<Result<Guid>>
{
  public required AppUserRequestDto AppUser { get; init; }
}