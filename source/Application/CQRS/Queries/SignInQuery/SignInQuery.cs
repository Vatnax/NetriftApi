using Netrift.Domain.Records;
using MediatR;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.SignInQuery;

/// <summary>
/// A CQRS query for handling signing-in a user.
/// </summary>
public sealed class SignInQuery : IRequest<Result<bool>>
{
  public required UserCredentials Credentials { get; init; }
}