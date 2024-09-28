using Netrift.Application.DataTransferObjects.RequestDataTransferObjects;
using Netrift.Domain.Core;
using MediatR;

namespace Netrift.Application.CQRS.Commands.CreateUserCommand;

/// <summary>
/// A CQRS command for creating a user.
/// </summary>
public sealed class CreateUserCommand
  : IRequest<Result<Guid>>
{
  public required AppUserRequestDto AppUser { get; init; }
}