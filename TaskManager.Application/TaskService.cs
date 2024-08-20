using FluentResults;
using AutoMapper;
using System.Net;
using TaskManager.DomainCore;
using AutoMapper.Internal.Mappers;
using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TaskManager.Application;

public class TaskService : ITaskService
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private const int CONST_MaxTaskByProject = 20;

    public TaskService(IMapper mapper, ITaskRepository taskRepository, IUserRepository userRepository, IProjectRepository projectRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<TaskDTO>> AddAsync(NewTaskDTO taskDto)
    {
        var task = _mapper.Map<DomainCore.Task>(taskDto);
        //Quando adicionada, o status da tarefa é sempre "new"
        task.State = (byte)TaskStateEnum.New;

        var user = await _userRepository.GetAsync(task.User);
        if (user is null)
            return Result.Fail("O usuário informado não existe.");

        var project = await _projectRepository.GetAsync(task.ProjectId);
        if (project is null)
            return Result.Fail("O projeto informado não existe.");

        var allTasksProject = (await _taskRepository.GetAllByProject(project.Id)).Count;
        if (allTasksProject >= CONST_MaxTaskByProject)
            return Result.Fail($"O limite máximo de {CONST_MaxTaskByProject} tarefas por projeto já foi atingido.");

        var result = await _taskRepository.AddAsync(task);

        if (result is null)
            return Result.Fail("Não foi possível criar o recurso.");

        return Result.Ok(_mapper.Map<TaskDTO>(result));
    }

    public async Task<Result<List<TaskDTO>>> GetAllByProjectAsync(int projectId)
    {
        var tasksProject = await _taskRepository.GetAllByProject(projectId);

        if (tasksProject is null)
            return Result.Fail("Não existem tarefas para o projeto informado.");

        var lstTasksProjectDto = _mapper.Map<List<TaskDTO>>(tasksProject);
        for (int i = 0; i < lstTasksProjectDto.Count; i++)
        {
            await SetTaskWithDetails(lstTasksProjectDto[i]);
        }

        return Result.Ok(lstTasksProjectDto);
    }

    public async Task<Result<TaskDTO>> RemoveAsync(int id)
    {
        var taskToDolete = await _taskRepository.GetAsync(id);

        if (taskToDolete is null)
            return Result.Fail("O recurso informado não existe.");

        var rowsAffected = await _taskRepository.RemoveAsync(taskToDolete.Id);

        if (rowsAffected != 1)
            return Result.Fail("Rollback");

        return Result.Ok(_mapper.Map<TaskDTO>(taskToDolete));
    }

    public async Task<Result<TaskDTO>> UpdateAsync(EditTaskDTO taskDto)
    {
        var taskToEdit = _mapper.Map<DomainCore.Task>(taskDto);

        var user = await _userRepository.GetAsync(taskToEdit.User);
        if (user is null)
            return Result.Fail("O usuário informado não existe.");

        //Partindo da premissa que a API terá autenticação, essa validação é desnecessária
        var userAction = await _userRepository.GetAsync(taskToEdit.User);
        if (userAction is null)
            return Result.Fail("O usuário informado no header não existe.");

        var project = await _projectRepository.GetAsync(taskToEdit.ProjectId);
        if (project is null)
            return Result.Fail("O projeto informado não existe.");

        var task = await _taskRepository.GetAsync(taskToEdit.Id);
        if (task is null)
            return Result.Fail("A tarefa informada não existe mais.");

        //Verifica se, caso atribuido novo projeto, o mesmo já não estourou o limite
        if (task.ProjectId != taskToEdit.ProjectId)
        {
            var allTasksProject = (await _taskRepository.GetAllByProject(taskToEdit.ProjectId)).Count;
            if (allTasksProject >= CONST_MaxTaskByProject)
                return Result.Fail($"O limite máximo de {CONST_MaxTaskByProject} tarefas para o projeto atribuído já foi atingido.");
        }

        var taskBeforeChange = await SetTaskWithDetails(_mapper.Map<TaskDTO>(task));
        var result = await _taskRepository.UpdateAsync(taskToEdit);

        if (result == 0)
            return Result.Fail("Não foi possível criar o recurso.");

        var taskChanged = await _taskRepository.GetAsync(taskToEdit.Id);
        var taskAfterChange = await SetTaskWithDetails(_mapper.Map<TaskDTO>(taskChanged));
        await LogHistory(taskDto.ActionUser, taskBeforeChange, taskAfterChange);

        return Result.Ok(_mapper.Map<TaskDTO>(taskAfterChange));
    }

    public async Task<Result<TaskDiscussionDTO>> CommentAsync(NewTaskDiscussionDTO taskDiscussionDto)
    {
        var newComment = _mapper.Map<TaskDiscussion>(taskDiscussionDto);
        var taskToComment = await _taskRepository.GetAsync(newComment.TaskId);

        if (taskToComment is null)
            return Result.Fail("A tarefa informada não existe mais.");

        var user = await _userRepository.GetAsync(newComment.User);
        if (user is null)
            return Result.Fail("O usuário informado não existe.");

        //Partindo da premissa que a API terá autenticação, essa validação é desnecessária
        var userAction = await _userRepository.GetAsync(taskDiscussionDto.ActionUser);
        if (userAction is null)
            return Result.Fail("O usuário informado no header não existe.");

        var taskBeforeChange = await SetTaskWithDetails(_mapper.Map<TaskDTO>(taskToComment));
        var result = await _taskRepository.CommentAsync(newComment);

        if (result is null)
            return Result.Fail("Não foi possível criar o recurso.");

        var taskCommented = await _taskRepository.GetAsync(result.TaskId);
        var taskAfterChange = await SetTaskWithDetails(_mapper.Map<TaskDTO>(taskCommented));
        await LogHistory(taskDiscussionDto.ActionUser, taskBeforeChange, taskAfterChange);

        return Result.Ok(_mapper.Map<TaskDiscussionDTO>(result));
    }

    private async Task<TaskDTO> SetTaskWithDetails(TaskDTO taskDto)
    {
        var lstDiscussion = await _taskRepository.GetDiscussionAsync(taskDto.Id);
        var lstHistory = await _taskRepository.GetHistoryAsync(taskDto.Id);

        var lstDiscussionDto = _mapper.Map<List<TaskDiscussionDTO>>(lstDiscussion);
        taskDto.Discussion.AddRange(lstDiscussionDto);

        var lstHistoryDto = _mapper.Map<List<TaskHistoryDTO>>(lstHistory);
        taskDto.ChangeHistory.AddRange(lstHistoryDto);

        return taskDto;
    }

    private async System.Threading.Tasks.Task LogHistory(string actionUser, TaskDTO before, TaskDTO after)
    {
        var changeHistory = new TaskHistory
        {
            ChangeDate = DateTime.Now,
            User = actionUser,
            TaskId = before.Id,
            BeforeChange = JsonConvert.SerializeObject(before),
            AfterChange = JsonConvert.SerializeObject(after)
        };

        await _taskRepository.AddChangeAsync(changeHistory);
    }
}
