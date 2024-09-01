using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.SignOutQuery;

public sealed class SignOutQueryHandler : IRequestHandler<SignOutQuery>
{
  private readonly IIdentityService _identityService;

  public SignOutQueryHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  public async Task Handle(SignOutQuery request, CancellationToken cancellationToken)
  {
    await _identityService.SignOut();
  }
}