using AutoMapper;
using TaskManager.DomainCore;

namespace TaskManager.Application
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProjectDTO, Project>().ReverseMap();
            CreateMap<NewProjectDTO, Project>().ReverseMap();
            CreateMap<TaskDTO, DomainCore.Task>().ReverseMap();
            CreateMap<NewTaskDTO, DomainCore.Task>().ReverseMap();
            CreateMap<EditTaskDTO, DomainCore.Task>().ReverseMap();
            CreateMap<TaskDiscussionDTO, DomainCore.TaskDiscussion>().ReverseMap();
            CreateMap<NewTaskDiscussionDTO, DomainCore.TaskDiscussion>().ReverseMap();
            CreateMap<TaskHistoryDTO, DomainCore.TaskHistory>().ReverseMap();
        }
    }
}
