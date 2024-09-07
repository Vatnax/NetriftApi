using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using AutoMapper;
using Netrift.Domain.Records;

namespace Netrift.Application.Mapping.MappingProfiles;

/// <summary>
/// A mapping profile for <see cref="AppUserResponseDto"/>
/// </summary>
public class UserDataMappingProfile : Profile
{
  /// <summary>
  /// Constructs the mapping profile.
  /// </summary>
  public UserDataMappingProfile() : base()
  {
    CreateMap<UserResponseData, AppUserResponseDto>();
  }
}