using FluentResults;
using AutoMapper;
using System.Net;
using TaskManager.DomainCore;
using AutoMapper.Internal.Mappers;

namespace TaskManager.Application;

public class TaskService : ITaskService
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;

    public TaskService(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<Result<ProjectDTO>> AddAsync(ProjectDTO projectDTO)
    {
        var project = _mapper.Map<Project>(projectDTO);
        var result = await _projectRepository.AddAsync(project);

        if (result is null)
            return Result.Fail("Não foi possível criar o recurso.");

        return Result.Ok(_mapper.Map<ProjectDTO>(result));
    }

    public async Task<Result<List<TaskDTO>>> GetAllByProjectAsync(int projectId)
    {
        var tasksProject = await _taskRepository.GetAllByProject(projectId);

        if (tasksProject is null)
            return Result.Fail("Não existem tarefas para o projeto informado.");

        var lstTasksProjectDto = _mapper.Map<List<TaskDTO>>(tasksProject);
        for (int i = 0; i < lstTasksProjectDto.Count; i++)
        {
            var taskDto = lstTasksProjectDto[i];

            var lstDiscussion = await _taskRepository.GetDiscussionAsync(taskDto.Id);
            var lstHistory = await _taskRepository.GetHistoryAsync(taskDto.Id);

            var lstDiscussionDto = _mapper.Map<List<TaskDiscussionDTO>>(lstDiscussion);
            taskDto.Discussion.AddRange(lstDiscussionDto);

            var lstHistoryDto = _mapper.Map<List<TaskHistoryDTO>>(lstHistory);
            taskDto.ChangeHistory.AddRange(lstHistoryDto);
        }

        return Result.Ok(lstTasksProjectDto);
    }

    public async Task<Result<ProjectDTO>> RemoveAsync(int id)
    {
        var projectToDelete = await _projectRepository.GetAsync(id);

        if (projectToDelete is null)
            return Result.Fail("O recurso informado não existe.");

        var lstTasksByProject = await _taskRepository.GetAllByProject(projectToDelete.Id);

        int tasksNewOrActive = lstTasksByProject.Count(t => t.State == (byte)TaskStateEnum.Active || t.State == (byte)TaskStateEnum.New);

        if (tasksNewOrActive > 0)
            return Result.Fail($"O projeto possui {tasksNewOrActive} tarefas não concluídas. É necessário concluí-las para remover o projeto.");

        var rowsAffected = await _projectRepository.RemoveAsync(projectToDelete.Id);

        if (rowsAffected != 1)
            return Result.Fail("Rollback");

        return Result.Ok(_mapper.Map<ProjectDTO>(projectToDelete));
    }
}
