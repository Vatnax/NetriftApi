using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using AutoMapper;
using Netrift.Domain.Enums;
using Netrift.Domain.Records;
using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;

/// <summary>
/// A CQRS query handler for getting a user.
/// </summary>
public class GetUserByEmailOrNameQueryHandler : IRequestHandler<GetUserByEmailOrNameQuery, Result<AppUserResponseDto>>
{
  private readonly IIdentityService _identityService;
  private readonly IMapper _mapper;

  /// <summary>
  /// Constructs the handler.
  /// </summary>
  /// <param name="identityService">An object that handles the identity.</param>
  /// <param name="mapper">An object that handles mapping between types.</param>
  public GetUserByEmailOrNameQueryHandler(IIdentityService identityService, IMapper mapper)
  {
    _identityService = identityService;
    _mapper = mapper;
  }


  /// <summary>
  /// Handles the query for getting a user.
  /// </summary>
  /// <param name="request">An object containing request information.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A <see cref="Task"/> with a generic <see cref="Result"/> object with either failure or success depending on the result.</returns>
  public async Task<Result<AppUserResponseDto>> Handle(GetUserByEmailOrNameQuery request, CancellationToken cancellationToken)
  {
    var user = await _identityService.GetUserByEmailOrNameAsync(request.EmailOrName);

    if (user is not null)
    {
      return Result<AppUserResponseDto>.Success(_mapper.Map<UserResponseData, AppUserResponseDto>(user));
    }

    return Result<AppUserResponseDto>.FailureWithoutErrorMessages(ErrorType.NotFound);
  }
}