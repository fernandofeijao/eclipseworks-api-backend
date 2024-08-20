using FluentResults;

namespace TaskManager.Application
{
    public interface ITaskService
    {
        Task<Result<List<TaskDTO>>> GetAllByProjectAsync(int projectId);
        Task<Result<TaskDTO>> AddAsync(NewTaskDTO taskDto);
        Task<Result<TaskDTO>> RemoveAsync(int id);
        Task<Result<TaskDTO>> UpdateAsync(EditTaskDTO taskDto);
        Task<Result<TaskDiscussionDTO>> CommentAsync(NewTaskDiscussionDTO taskDiscussionDto);
    }
}
