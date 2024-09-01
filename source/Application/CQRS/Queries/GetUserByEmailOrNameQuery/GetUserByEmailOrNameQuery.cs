using Application.DataTransferObjects.ResponseDataTransferObjects;
using MediatR;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;

public class GetUserByEmailOrNameQuery : IRequest<Result<AppUserResponseDto>>
{
  public required string EmailOrName { get; init; }
}