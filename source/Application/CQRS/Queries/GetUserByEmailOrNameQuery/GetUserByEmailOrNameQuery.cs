using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using MediatR;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;

/// <summary>
/// A CQRS query for getting a user.
/// </summary>
public class GetUserByEmailOrNameQuery : IRequest<Result<AppUserResponseDto>>
{
  public required string EmailOrName { get; init; }
}