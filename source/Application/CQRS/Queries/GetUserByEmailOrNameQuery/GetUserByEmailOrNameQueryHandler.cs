using Application.DataTransferObjects.ResponseDataTransferObjects;
using AutoMapper;
using Domain.Enums;
using Domain.Records;
using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.GetUserByEmailOrNameQuery;

public class GetUserByEmailOrNameQueryHandler : IRequestHandler<GetUserByEmailOrNameQuery, Result<AppUserResponseDto>>
{
  private readonly IIdentityService _identityService;
  private readonly IMapper _mapper;

  public GetUserByEmailOrNameQueryHandler(IIdentityService identityService, IMapper mapper)
  {
    _identityService = identityService;
    _mapper = mapper;
  }

  public async Task<Result<AppUserResponseDto>> Handle(GetUserByEmailOrNameQuery request, CancellationToken cancellationToken)
  {
    var user = await _identityService.GetUserByEmailOrName(request.EmailOrName);

    if (user is not null)
    {
      return Result<AppUserResponseDto>.Success(_mapper.Map<UserResponseData, AppUserResponseDto>(user));
    }

    return Result<AppUserResponseDto>.Failure(["User not found!"], ErrorType.NotFound);
  }
}