using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using MediatR;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.GetUserByIdQuery;

/// <summary>
/// A CQRS query for getting a user.
/// </summary>
public class GetUserByIdQuery : IRequest<Result<AppUserResponseDto>>
{
  public required Guid UserId { get; init; }
}