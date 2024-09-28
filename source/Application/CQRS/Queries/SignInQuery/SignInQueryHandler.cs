using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.SignInQuery;

/// <summary>
/// A CQRS query handler for signing-in a user.
/// </summary>
public sealed class SignInQueryHandler : IRequestHandler<SignInQuery, Result<bool>>
{
  private readonly IIdentityService _identityService;

  /// <summary>
  /// Constructs the handler.
  /// </summary>
  /// <param name="identityService">An object that handles the identity.</param>
  public SignInQueryHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  /// <summary>
  /// Handles the query for signing-in a user.
  /// </summary>
  /// <param name="request">An object containing request information.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A <see cref="Task"/> with a generic <see cref="Result"/> object with either failure or success depending on the result.</returns>
  public async Task<Result<bool>> Handle(SignInQuery request, CancellationToken cancellationToken)
  {
    var succeeded = await _identityService.SignInAsync(request.Credentials);

    if (succeeded)
    {
      return Result<bool>.Success(succeeded);
    }

    return Result<bool>.Failure(["Sign in failed. Please verify your credentials"]);
  }
}