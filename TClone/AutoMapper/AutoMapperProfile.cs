using AutoMapper;
using Dto;
using Models;

namespace TClone.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
            CreateMap<User, UserDto>();
            CreateMap<Profile, ProfileDto>();
            CreateMap<ProfileDto,Profile>();

        }
    }
}
