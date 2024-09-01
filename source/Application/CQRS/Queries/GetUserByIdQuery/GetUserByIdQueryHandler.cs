using Application.DataTransferObjects.ResponseDataTransferObjects;
using AutoMapper;
using Domain.Enums;
using Domain.Records;
using MediatR;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Domain.Core;

namespace Netrift.Application.CQRS.Queries.GetUserByIdQuery;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<AppUserResponseDto>>
{
  private readonly IIdentityService _identityService;
  private readonly IMapper _mapper;

  public GetUserByIdQueryHandler(IIdentityService identityService, IMapper mapper)
  {
    _identityService = identityService;
    _mapper = mapper;
  }

  public async Task<Result<AppUserResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var user = await _identityService.GetUserById(request.UserId);

    if (user is not null)
    {
      return Result<AppUserResponseDto>.Success(_mapper.Map<UserResponseData, AppUserResponseDto>(user));
    }

    return Result<AppUserResponseDto>.Failure(["User not found!"], ErrorType.NotFound);
  }
}