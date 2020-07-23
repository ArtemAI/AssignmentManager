using AutoMapper;
using BLL.Models;
using DAL.Entities;

namespace BLL
{
    /// <summary>
    /// Provides a set of mapping configurations for AutoMapper.
    /// </summary>
    internal class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Assignment, AssignmentDto>().ReverseMap();
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<RegisterUserDto, UserProfile>();
            CreateMap<RegisterUserDto, ApplicationUser>();
        }
    }
}