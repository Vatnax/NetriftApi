using Netrift.Application.DataTransferObjects.ResponseDataTransferObjects;
using AutoMapper;
using Netrift.Domain.Records;
using FluentAssertions;
using Netrift.Application.Mapping.MappingProfiles;

namespace Netrift.Tests.MappingProfiles;

public class UserDataMappingProfileTests
{
  private readonly IConfigurationProvider _config;
  private readonly IMapper _mapper;
  public UserDataMappingProfileTests()
  {
    _config = new MapperConfiguration(cfg => cfg.AddProfile<UserDataMappingProfile>());
    _mapper = _config.CreateMapper();
  }

  [Fact]
  public void MapperProfile_Configuration_IsValid()
  {
    // Arrange

    var config = new MapperConfiguration(cfg => cfg.AddProfile<UserDataMappingProfile>());

    // Act & Assert

    config.AssertConfigurationIsValid();
  }

  [Fact]
  public void Map_MapsProperly_FromTypeToType()
  {
    // Arrange

    UserResponseData userResponseData = new(Guid.NewGuid(), "mock", "mock@email.com");

    // Act

    var result = _mapper.Map<UserResponseData, AppUserResponseDto>(userResponseData);

    // Assert

    result.Id.Should().Be(userResponseData.Id);
    result.UserName.Should().BeEquivalentTo(userResponseData.UserName);
    result.Email.Should().BeEquivalentTo(userResponseData.Email);
  }

}