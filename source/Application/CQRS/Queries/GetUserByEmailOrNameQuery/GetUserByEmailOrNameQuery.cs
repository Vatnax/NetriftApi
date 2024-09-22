using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using MediatR;
using Netrift.Domain.Core;
using Netrift.Domain.Helpers;
using Netrift.Domain.Constants;

namespace Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;

/// <summary>
/// A CQRS query for getting a user.
/// </summary>
public class GetUserByEmailOrNameQuery : IRequest<Result<AppUserResponseDto>>, ICacheable
{
  public required string EmailOrName { get; init; }

  public string CacheKey => CacheHelper.CreateCacheKey(CacheConstants.UserCachePrefix, EmailOrName);

  public TimeSpan ExpirationTime => CacheConstants.DefaultCacheExpirationTime;
}