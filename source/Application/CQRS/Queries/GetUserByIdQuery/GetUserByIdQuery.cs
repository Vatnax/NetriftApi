using Application.DataTransferObjects.ResponseDataTransferObjects;
using MediatR;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.GetUserByIdQuery;

public class GetUserByIdQuery : IRequest<Result<AppUserResponseDto>>
{
  public required Guid UserId { get; init; }
}