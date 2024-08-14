using FluentResults;
using AutoMapper;
using System.Net;
using TaskManager.DomainCore;
using AutoMapper.Internal.Mappers;

namespace TaskManager.Application;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper, ITaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
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
    public async Task<Result<List<ProjectDTO>>> GetAllAsync(string user)
    {
        var lstProjects = await _projectRepository.GetAllAsync(user);
        var lstProjectDto = _mapper.Map<List<ProjectDTO>>(lstProjects);
        return Result.Ok(lstProjectDto);
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
