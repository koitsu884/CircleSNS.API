using AutoMapper;
using CircleSNS.API.Dto;
using CircleSNS.API.Models;

namespace CircleSNS.API.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
          CreateMap<RegistrationDto,AppUser>();
          CreateMap<RegistrationDto,Member>();
          CreateMap<CreateUserDto,Member>();
          CreateMap<CreateUserDto,AppUser>();
          CreateMap<UpdateMemberDto,AppUser>();
          CreateMap<UpdateMemberDto,Member>();
        }
    }
}