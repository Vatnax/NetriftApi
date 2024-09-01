using Domain.Records;
using MediatR;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.SignInQuery;

public sealed class SignInQuery : IRequest<Result<bool>>
{
  public required UserCredentials Credentials { get; init; }
}