namespace Application.CQRS.Commands.CreateUserCommand;

using System.Diagnostics.CodeAnalysis;
using Application.DataTransferObjects.RequestDataTransferObjects;
using Application.DataTransferObjects.ResponseDataTransferObjects;
using Netrift.Domain.Core;
using MediatR;

public sealed class CreateUserCommand
  : IRequest<Result<Guid, IEnumerable<string>>>
{
  public required AppUserRequestDto AppUser { get; init; }
}