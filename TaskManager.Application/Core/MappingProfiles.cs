using AutoMapper;
using TaskManager.DomainCore;

namespace TaskManager.Application
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProjectDTO, Project>().ReverseMap();
        }
    }
}
