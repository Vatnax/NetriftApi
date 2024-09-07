using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;

namespace Netrift.Application.CQRS.Queries.SignOutQuery;

/// <summary>
/// A CQRS query handler for signing-out a user.
/// </summary>
public sealed class SignOutQueryHandler : IRequestHandler<SignOutQuery>
{
  private readonly IIdentityService _identityService;

  /// <summary>
  ///
  /// </summary>
  /// <param name="identityService"></param>
  public SignOutQueryHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }

  /// <summary>
  /// Handles the query for signing-out a user.
  /// </summary>
  /// <param name="request">An object containing request information.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A <see cref="Task"/> object determining that the sign-out process has been successful.</returns>
  public async Task Handle(SignOutQuery request, CancellationToken cancellationToken)
  {
    await _identityService.SignOutAsync();
  }
}