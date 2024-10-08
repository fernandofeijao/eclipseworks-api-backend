using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentResults;
using NSubstitute;
using TaskManager.DomainCore;
using Xunit;

namespace TaskManager.Application.Test.Tests
{
    public class TaskUnitTest
    {
        private readonly IMapper _mapper;

        public TaskUnitTest()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(MappingProfiles).Assembly);
            });

            _mapper = config.CreateMapper();
        }

        #region Happy Way

        [Fact(DisplayName = "GetAllByProjectAsync quando executado com sucesso deve retornar todos as tarefas de um projeto.")]
        [Trait("TaskService.GetAllByProjectAsync", "Unit")]
        public async System.Threading.Tasks.Task GiveAProject_WhenGetAllByProjectAsyncSuccessCalled_ThenReturnAllTasksByProject()
        {
            //Arrange
            var userRepoMock = Substitute.For<IUserRepository>();
            var taskRepoMock = Substitute.For<ITaskRepository>();
            var projectRepoMock = Substitute.For<IProjectRepository>();

            var projectIdFake = new Faker("pt_BR").Random.Int(0);

            var lstTasksFake = new Faker<DomainCore.Task>()
                .RuleFor(p => p.Id, f => f.Random.Int(0))
                .RuleFor(p => p.User, f => f.Internet.UserName())
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.State, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.Priority, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.TargetDate, f => f.Date.Past())
                .RuleFor(p => p.ProjectId, projectIdFake)
            .Generate(20);

            taskRepoMock.GetAllByProject(Arg.Any<int>()).Returns<List<DomainCore.Task>>(lstTasksFake);

            var lstDiscussionFake = new Faker<DomainCore.TaskDiscussion>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.Comment, f => f.Lorem.Text())
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.TaskId, f => f.Random.Int(0))
            .Generate(10);

            taskRepoMock.GetDiscussionAsync(Arg.Any<int>()).Returns<List<DomainCore.TaskDiscussion>>(lstDiscussionFake);

            var lstHistoryFake = new Faker<DomainCore.TaskHistory>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.TaskId, f => f.Random.Int(0))
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.ChangeDate, f => f.Date.Past())
                .RuleFor(td => td.BeforeChange, f => f.Lorem.Text())
                .RuleFor(td => td.AfterChange, f => f.Lorem.Text())
            .Generate(10);

            taskRepoMock.GetHistoryAsync(Arg.Any<int>()).Returns<List<DomainCore.TaskHistory>>(lstHistoryFake);

            var taskService = new TaskService(_mapper, taskRepoMock, userRepoMock, projectRepoMock);

            //Act
            var result = await taskService.GetAllByProjectAsync(projectIdFake);

            //Assert
            Assert.IsType<Result<List<TaskDTO>>>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact(DisplayName = "RemoveAsync quando executado com sucesso deve remover a tarefa e retornar result com a tarefa deletada.")]
        [Trait("TaskService.RemoveAsync", "Unit")]
        public async System.Threading.Tasks.Task GiveATask_WhenRemoveAsyncSuccessCalled_ThenReturnTaskDTO()
        {
            //Arrange
            var userRepoMock = Substitute.For<IUserRepository>();
            var taskRepoMock = Substitute.For<ITaskRepository>();
            var projectRepoMock = Substitute.For<IProjectRepository>();

            var taskIdFake = new Faker("pt_BR").Random.Int(0);

            var taskToDelete = new Faker<DomainCore.Task>()
                .RuleFor(p => p.Id, taskIdFake)
                .RuleFor(p => p.User, f => f.Internet.UserName())
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.State, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.Priority, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.TargetDate, f => f.Date.Past())
                .RuleFor(p => p.ProjectId, f => f.Random.Int(0))
            .Generate();

            taskRepoMock.GetAsync(Arg.Any<int>()).Returns(taskToDelete);
            taskRepoMock.RemoveAsync(Arg.Any<int>()).Returns(1);

            var taskService = new TaskService(_mapper, taskRepoMock, userRepoMock, projectRepoMock);

            //Act
            var result = await taskService.RemoveAsync(taskIdFake);

            //Assert
            Assert.IsType<Result<TaskDTO>>(result);
            Assert.True(result.IsSuccess);
            result.Value.Should().BeEquivalentTo(taskToDelete);
        }

        [Fact(DisplayName = "AddAsync quando executado com sucesso deve criar a tarefa e retorna-la como result.")]
        [Trait("TaskService.AddAsync", "Unit")]
        public async System.Threading.Tasks.Task GiveANewTaskDTO_WhenAddAsyncSuccessCalled_ThenReturnTaskDTOInserted()
        {
            //Arrange
            var userRepoMock = Substitute.For<IUserRepository>();
            var taskRepoMock = Substitute.For<ITaskRepository>();
            var projectRepoMock = Substitute.For<IProjectRepository>();

            var usernameFake = new Faker().Internet.UserName();
            var projectIdFake = new Faker().Random.Int(0);

            var newTaskDto = new Faker<NewTaskDTO>()
                .RuleFor(p => p.User, usernameFake)
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.Priority, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.TargetDate, f => f.Date.Past())
                .RuleFor(p => p.ProjectId, projectIdFake)
            .Generate();

            var userFake = new Faker<DomainCore.User>()
                .RuleFor(u => u.Id, usernameFake);

            userRepoMock.GetAsync(Arg.Any<string>()).Returns(userFake);

            var projectFake = new Faker<DomainCore.Project>()
                .RuleFor(p => p.Id, projectIdFake);

            projectRepoMock.GetAsync(Arg.Any<int>()).Returns(projectFake);

            var lstTasksByProject = new Faker<List<DomainCore.Task>>().Generate();

            taskRepoMock.GetAllByProject(Arg.Any<int>()).Returns(lstTasksByProject);

            var taskIdFake = new Faker().Random.Int(0);
            var taskInserted = new Faker<DomainCore.Task>()
                .RuleFor(t => t.Id, taskIdFake)
                .RuleFor(t => t.User, usernameFake)
                .RuleFor(t => t.Title, newTaskDto.Title)
                .RuleFor(t => t.Description, newTaskDto.Description)
                .RuleFor(t => t.State, (byte)TaskStateEnum.New)
                .RuleFor(t => t.Priority, newTaskDto.Priority)
                .RuleFor(t => t.TargetDate, newTaskDto.TargetDate)
                .RuleFor(t => t.ProjectId, newTaskDto.ProjectId)
            .Generate();

            taskRepoMock.AddAsync(Arg.Any<DomainCore.Task>()).Returns(taskInserted);

            var taskService = new TaskService(_mapper, taskRepoMock, userRepoMock, projectRepoMock);

            //Act
            var result = await taskService.AddAsync(newTaskDto);

            //Assert
            Assert.IsType<Result<TaskDTO>>(result);
            Assert.True(result.IsSuccess);
            result.Value.Id.Should().Be(taskIdFake);
            result.Value.User.Should().Be(usernameFake);
            result.Value.Title.Should().Be(newTaskDto.Title);
            result.Value.Description.Should().Be(newTaskDto.Description);
            result.Value.State.Should().Be((byte)TaskStateEnum.New);
            result.Value.Priority.Should().Be(newTaskDto.Priority);
            result.Value.TargetDate.Should().Be(newTaskDto.TargetDate);
            result.Value.ProjectId.Should().Be(newTaskDto.ProjectId);
        }

        [Fact(DisplayName = "AddAsync quando executado deve validar o número de tarefas do projeto utilizado e retornar uma mensagem de falha caso já existam 20 tarefas num projeto.")]
        [Trait("TaskService.AddAsync", "Unit")]
        public async System.Threading.Tasks.Task GiveANewTaskDTO_WhenAddAsyncCalledButProjectOverThanTwentyTasks_ThenReturnFailMessage()
        {
            //Arrange
            var userRepoMock = Substitute.For<IUserRepository>();
            var taskRepoMock = Substitute.For<ITaskRepository>();
            var projectRepoMock = Substitute.For<IProjectRepository>();

            var usernameFake = new Faker().Internet.UserName();
            var projectIdFake = new Faker().Random.Int(0);

            var newTaskDto = new Faker<NewTaskDTO>()
                .RuleFor(p => p.User, usernameFake)
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.Priority, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.TargetDate, f => f.Date.Past())
                .RuleFor(p => p.ProjectId, projectIdFake)
            .Generate();

            var userFake = new Faker<DomainCore.User>()
                .RuleFor(u => u.Id, usernameFake);

            userRepoMock.GetAsync(Arg.Any<string>()).Returns(userFake);

            var projectFake = new Faker<DomainCore.Project>()
                .RuleFor(p => p.Id, projectIdFake);

            projectRepoMock.GetAsync(Arg.Any<int>()).Returns(projectFake);

            var lstTasksByProject = new Faker<DomainCore.Task>()
                .RuleFor(t => t.Id, f => f.Random.Int(0))
            .Generate(20);

            taskRepoMock.GetAllByProject(Arg.Any<int>()).Returns(lstTasksByProject);

            var taskService = new TaskService(_mapper, taskRepoMock, userRepoMock, projectRepoMock);

            //Act
            var result = await taskService.AddAsync(newTaskDto);

            //Assert
            Assert.IsType<Result<TaskDTO>>(result);
            Assert.True(result.IsFailed);
            result.Reasons[0].Message.Should().Be($"O limite máximo de {lstTasksByProject.Count} tarefas por projeto já foi atingido.");
        }

        [Fact(DisplayName = "UpdateAsync quando executado com sucesso deve editar a tarefa e retornar taskDto como result.")]
        [Trait("TaskService.UpdateAsync", "Unit")]
        public async System.Threading.Tasks.Task GiveATaskId_WhenUpdateAsyncSuccessCalled_ThenReturnTaskDTOUpdated()
        {
            //Arrange
            var userRepoMock = Substitute.For<IUserRepository>();
            var taskRepoMock = Substitute.For<ITaskRepository>();
            var projectRepoMock = Substitute.For<IProjectRepository>();

            var usernameFake = new Faker().Internet.UserName();
            var usernameActionFake = new Faker().Internet.UserName();
            var projectIdFake = new Faker().Random.Int(0);
            var taskIdFake = new Faker().Random.Int(0);

            var editTaskDto = new Faker<EditTaskDTO>()
                .RuleFor(p => p.Id, taskIdFake)
                .RuleFor(p => p.User, usernameFake)
                .RuleFor(p => p.ActionUser, usernameActionFake)
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.State, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.Priority, f => (byte)f.Random.Int(1, 3))
                .RuleFor(p => p.TargetDate, f => f.Date.Past())
                .RuleFor(p => p.ProjectId, projectIdFake)
            .Generate();

            //First call to check user assigned
            var userFake = new Faker<DomainCore.User>()
                .RuleFor(u => u.Id, usernameFake)
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Profile, (byte)UserProfileEnum.Normal)
            .Generate();

            //Second call to check user action
            var userActionFake = new Faker<DomainCore.User>()
                .RuleFor(u => u.Id, usernameActionFake)
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Profile, (byte)UserProfileEnum.Manager)
            .Generate();

            userRepoMock.GetAsync(Arg.Any<string>()).Returns(userFake, userActionFake);

            var projectFake = new Faker<DomainCore.Project>()
                .RuleFor(p => p.Id, projectIdFake)
            .Generate();

            projectRepoMock.GetAsync(Arg.Any<int>()).Returns(projectFake);

            //First call to get history and discussion
            var taskBeforeChange = new Faker<DomainCore.Task>()
                .RuleFor(t => t.Id, taskIdFake)
                .RuleFor(t => t.User, f => f.Internet.UserName())
                .RuleFor(t => t.Title, f => f.Lorem.Word())
                .RuleFor(t => t.Description, f => f.Lorem.Text())
                .RuleFor(t => t.State, f => (byte)f.Random.Int(1, 3))
                .RuleFor(t => t.Priority, f => (byte)f.Random.Int(1, 3))
                .RuleFor(t => t.TargetDate, f => f.Date.Future())
                .RuleFor(t => t.ProjectId, f => editTaskDto.ProjectId)
            .Generate();

            var taskToEditFake = new Faker<DomainCore.Task>()
                .RuleFor(t => t.Id, taskIdFake)
                .RuleFor(t => t.User, editTaskDto.User)
                .RuleFor(t => t.Title, editTaskDto.Title)
                .RuleFor(t => t.Description, editTaskDto.Description)
                .RuleFor(t => t.State, editTaskDto.State)
                .RuleFor(t => t.Priority, editTaskDto.Priority)
                .RuleFor(t => t.TargetDate, editTaskDto.TargetDate)
                .RuleFor(t => t.ProjectId, editTaskDto.ProjectId)
            .Generate();

            taskRepoMock.GetAsync(Arg.Any<int>()).Returns(taskBeforeChange, taskToEditFake);

            var lstDiscussionFake = new Faker<DomainCore.TaskDiscussion>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.Comment, f => f.Lorem.Text())
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.TaskId, taskIdFake)
            .Generate(10);

            taskRepoMock.GetDiscussionAsync(Arg.Any<int>()).Returns<List<DomainCore.TaskDiscussion>>(lstDiscussionFake);

            var lstHistoryFake = new Faker<DomainCore.TaskHistory>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.TaskId, taskIdFake)
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.ChangeDate, f => f.Date.Past())
                .RuleFor(td => td.BeforeChange, f => f.Lorem.Text())
                .RuleFor(td => td.AfterChange, f => f.Lorem.Text())
            .Generate(10);

            var lstNewHistoryFake = new List<DomainCore.TaskHistory>(lstHistoryFake);

            var newLogHistoryFake = new Faker<DomainCore.TaskHistory>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.TaskId, taskIdFake)
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.ChangeDate, f => f.Date.Past())
                .RuleFor(td => td.BeforeChange, f => f.Lorem.Text())
                .RuleFor(td => td.AfterChange, f => f.Lorem.Text())
            .Generate();

            lstNewHistoryFake.Add(newLogHistoryFake);

            taskRepoMock.GetHistoryAsync(Arg.Any<int>()).Returns<List<DomainCore.TaskHistory>>(lstHistoryFake, lstNewHistoryFake);
            taskRepoMock.UpdateAsync(Arg.Any<DomainCore.Task>()).Returns(1);

            var taskService = new TaskService(_mapper, taskRepoMock, userRepoMock, projectRepoMock);

            //Act
            var result = await taskService.UpdateAsync(editTaskDto);

            //Assert
            Assert.IsType<Result<TaskDTO>>(result);
            Assert.True(result.IsSuccess);
            result.Value.ChangeHistory.Count.Should().Be(lstNewHistoryFake.Count);
            result.Value.User.Should().NotBe(taskBeforeChange.User);
            result.Value.Title.Should().NotBe(taskBeforeChange.Title);
            result.Value.Description.Should().NotBe(taskBeforeChange.Description);
            result.Value.TargetDate.Should().NotBe(taskBeforeChange.TargetDate);
        }

        [Fact(DisplayName = "CommentAsync quando executado com sucesso deve inserir um novo comentário na discussão da tarefa e retornar TaskDiscussionDTO como result.")]
        [Trait("TaskService.CommentAsync", "Unit")]
        public async System.Threading.Tasks.Task GiveANewTaskDiscussionDTO_WhenCommentAsyncSuccessCalled_ThenReturnTaskDiscussionDTOInserted()
        {
            //Arrange
            var userRepoMock = Substitute.For<IUserRepository>();
            var taskRepoMock = Substitute.For<ITaskRepository>();
            var projectRepoMock = Substitute.For<IProjectRepository>();

            var usernameFake = new Faker().Internet.UserName();
            var taskIdFake = new Faker().Random.Int(0);

            var newTaskDiscussionFake = new Faker<NewTaskDiscussionDTO>()
                .RuleFor(p => p.TaskId, taskIdFake)
                .RuleFor(p => p.User, usernameFake)
                .RuleFor(p => p.ActionUser, usernameFake)
                .RuleFor(p => p.Comment, f => f.Lorem.Text())
            .Generate();

            var taskToCommentFake = new Faker<DomainCore.Task>()
                .RuleFor(t => t.Id, taskIdFake)
                .RuleFor(t => t.User, f => f.Internet.UserName())
                .RuleFor(t => t.Title, f => f.Lorem.Word())
                .RuleFor(t => t.Description, f => f.Lorem.Text())
                .RuleFor(t => t.State, f => (byte)f.Random.Int(1, 3))
                .RuleFor(t => t.Priority, f => (byte)f.Random.Int(1, 3))
                .RuleFor(t => t.TargetDate, f => f.Date.Future())
                .RuleFor(t => t.ProjectId, f => f.Random.Int(0))
            .Generate();

            taskRepoMock.GetAsync(Arg.Any<int>()).Returns(taskToCommentFake);

            var userFake = new Faker<DomainCore.User>()
                .RuleFor(u => u.Id, usernameFake)
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Profile, f => (byte)f.Random.Int(1, 2))
            .Generate();

            userRepoMock.GetAsync(Arg.Any<string>()).Returns(userFake);

            var lstDiscussionBeforeCommentFake = new Faker<TaskDiscussion>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.Comment, f => f.Lorem.Text())
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.TaskId, taskIdFake)
            .Generate(10);

            var lstDiscussionAfterCommentFake = new List<TaskDiscussion>(lstDiscussionBeforeCommentFake);
            var newComment = _mapper.Map<TaskDiscussion>(newTaskDiscussionFake);
            lstDiscussionAfterCommentFake.Add(newComment);

            taskRepoMock.GetDiscussionAsync(Arg.Any<int>()).Returns<List<DomainCore.TaskDiscussion>>(lstDiscussionBeforeCommentFake, lstDiscussionAfterCommentFake);

            var lstHistoryFake = new Faker<DomainCore.TaskHistory>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.TaskId, taskIdFake)
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.ChangeDate, f => f.Date.Past())
                .RuleFor(td => td.BeforeChange, f => f.Lorem.Text())
                .RuleFor(td => td.AfterChange, f => f.Lorem.Text())
            .Generate(10);

            var lstNewHistoryFake = new List<DomainCore.TaskHistory>(lstHistoryFake);

            var newLogHistoryFake = new Faker<DomainCore.TaskHistory>()
                .RuleFor(td => td.Id, f => f.Random.Int(0))
                .RuleFor(td => td.TaskId, taskIdFake)
                .RuleFor(td => td.User, f => f.Internet.UserName())
                .RuleFor(td => td.ChangeDate, f => f.Date.Past())
                .RuleFor(td => td.BeforeChange, f => f.Lorem.Text())
                .RuleFor(td => td.AfterChange, f => f.Lorem.Text())
            .Generate();

            lstNewHistoryFake.Add(newLogHistoryFake);

            taskRepoMock.GetHistoryAsync(Arg.Any<int>()).Returns<List<DomainCore.TaskHistory>>(lstHistoryFake, lstNewHistoryFake);
            taskRepoMock.CommentAsync(Arg.Any<TaskDiscussion>()).Returns(newComment);

            var taskService = new TaskService(_mapper, taskRepoMock, userRepoMock, projectRepoMock);

            //Act
            var result = await taskService.CommentAsync(newTaskDiscussionFake);

            //Assert
            Assert.IsType<Result<TaskDiscussionDTO>>(result);
            Assert.True(result.IsSuccess);
        }
        #endregion

        #region Exceptions

        [Fact(DisplayName = "GetAllByProjectAsync quando executado com sucesso deve retornar todos as tarefas de um projeto.")]
        [Trait("TaskService.GetAllByProjectAsync", "Unit")]
        public async System.Threading.Tasks.Task GiveAProject_WhenGetAllByProjectAsyncCalledButProjectNotExistsOrTasksNotExistsForProject_ThenReturnFailMessage()
        {
            //Arrange
            var userRepoMock = Substitute.For<IUserRepository>();
            var taskRepoMock = Substitute.For<ITaskRepository>();
            var projectRepoMock = Substitute.For<IProjectRepository>();

            var projectIdFake = new Faker("pt_BR").Random.Int(0);

            var taskService = new TaskService(_mapper, taskRepoMock, userRepoMock, projectRepoMock);

            //Act
            var result = await taskService.GetAllByProjectAsync(projectIdFake);

            //Assert
            Assert.IsType<Result<List<TaskDTO>>>(result);
            Assert.True(result.IsFailed);
            result.Reasons[0].Message.Should().Be("Não existem tarefas para o projeto informado.");
        }

        #endregion
    }
}