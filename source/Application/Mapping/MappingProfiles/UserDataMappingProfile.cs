using Application.DataTransferObjects.ResponseDataTransferObjects;
using AutoMapper;
using Domain.Records;

namespace Netrift.Application.Mapping.MappingProfiles;

public class UserDataMappingProfile : Profile
{
  public UserDataMappingProfile() : base()
  {
    CreateMap<UserResponseData, AppUserResponseDto>();
  }
}