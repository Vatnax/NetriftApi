using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using MediatR;
using Netrift.Domain.Core;
using Netrift.Domain.Helpers;
using Netrift.Domain.Constants;

namespace Netrift.Application.CQRS.Queries.GetUserByIdQuery;

/// <summary>
/// A CQRS query for getting a user.
/// </summary>
public class GetUserByIdQuery : IRequest<Result<AppUserResponseDto>>, ICacheable
{
  public required Guid UserId { get; init; }

  public string CacheKey => CacheHelper.CreateCacheKey(CacheConstants.UserCachePrefix, UserId.ToString());

  public TimeSpan ExpirationTime => CacheConstants.DefaultCacheExpirationTime;
}