using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.SignInQuery;

public sealed class SignOInQueryHandler : IRequestHandler<SignInQuery, Result<bool>>
{
  private readonly IIdentityService _identityService;

  public SignOInQueryHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  public async Task<Result<bool>> Handle(SignInQuery request, CancellationToken cancellationToken)
  {
    var succeeded = await _identityService.SignIn(request.Credentials);

    if (succeeded)
    {
      return Result<bool>.Success(succeeded);
    }

    return Result<bool>.Failure(["Sign in failed. Please verify your credentials"]);
  }
}