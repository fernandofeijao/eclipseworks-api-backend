using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Moq;
using NSubstitute;
using TaskManager.DomainCore;

namespace TaskManager.Application.Test;

public class ProjectUnitTest
{
    private readonly IMapper _mapper;

    public ProjectUnitTest()
    {
        var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(MappingProfiles).Assembly);
            });

        _mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "GetAllAsync quando executado com sucesso deve retornar todos os projetos de um usuário.")]
    [Trait("ProjectService.GetAsync", "Unit")]
    public async System.Threading.Tasks.Task GiveAnUser_WhenSuccessCalled_ThenReturnAllProjectsByUser()
    {
        //Arrange
        var userRepoMock = Substitute.For<IUserRepository>();
        var taskRepoMock = Substitute.For<ITaskRepository>();
        var projectRepoMock = Substitute.For<IProjectRepository>();

        var listProjects = new Faker<Project>("pt_BR")
            .RuleFor(p => p.Id, f => f.Random.Int())
            .RuleFor(p => p.Name, f => f.Lorem.Word())
            .RuleFor(p => p.Owner, f => f.Internet.UserName())
            .RuleFor(p => p.CreateDate, f => f.Date.Recent())
            .RuleFor(p => p.StartDate, f => f.Date.Future())
            .RuleFor(p => p.FinishDate, f => f.Date.Future()).Generate(10);

        var projectsDto = _mapper.Map<List<ProjectDTO>>(listProjects);
        projectRepoMock.GetAllAsync(It.IsAny<string>()).Returns<List<Project>>(listProjects);

        var projectService = new ProjectService(projectRepoMock, _mapper, taskRepoMock, userRepoMock);

        //Act
        var result = await projectService.GetAllAsync(It.IsAny<string>());

        //Assert
        Assert.IsType<Result<List<ProjectDTO>>>(result);
        Assert.True(result.IsSuccess);
        projectsDto.Should().BeEquivalentTo(result.Value);
    }

    [Fact(DisplayName = "AddAsync quando executado com sucesso deve incluir um novo projeto e retorná-lo como resultado.")]
    [Trait("ProjectService.AddAsync", "Unit")]
    public async System.Threading.Tasks.Task GivenAValidNewProjectDto_WhenSuccessAddAsyncCalled_ThenReturnAProjectDtoInserted()
    {
        //Arrange
        var userRepoMock = Substitute.For<IUserRepository>();
        var taskRepoMock = Substitute.For<ITaskRepository>();
        var projectRepoMock = Substitute.For<IProjectRepository>();

        var usernameFake = new Faker("pt_BR").Internet.UserName();
        var existsUser = new Faker<User>("pt_BR")
            .RuleFor(u => u.Id, usernameFake).Generate();

        var newProjectDto = new Faker<NewProjectDTO>("pt_BR")
            .RuleFor(p => p.Name, f => f.Lorem.Word())
            .RuleFor(p => p.Owner, usernameFake)
            .RuleFor(p => p.StartDate, f => f.Date.Future())
            .RuleFor(p => p.FinishDate, f => f.Date.Future()).Generate();

        var projectInserted = new Faker<Project>("pt_BR")
            .RuleFor(p => p.Id, f => f.Random.Int(0))
            .RuleFor(p => p.Name, newProjectDto.Name)
            .RuleFor(p => p.Owner, newProjectDto.Owner)
            .RuleFor(p => p.CreateDate, DateTime.Now)
            .RuleFor(p => p.StartDate, newProjectDto.StartDate)
            .RuleFor(p => p.FinishDate, newProjectDto.FinishDate).Generate();

        userRepoMock.GetAsync(Arg.Any<string>()).Returns<User?>(existsUser);
        var project = _mapper.Map<Project>(newProjectDto);
        projectRepoMock.AddAsync(Arg.Any<Project>()).Returns<Project>(projectInserted);

        var projectService = new ProjectService(projectRepoMock, _mapper, taskRepoMock, userRepoMock);

        //Act
        var result = await projectService.AddAsync(newProjectDto);

        //Assert
        Assert.IsType<Result<ProjectDTO>>(result);
        Assert.True(result.IsSuccess);
        result.Value.Name.Should().Be(newProjectDto.Name);
        result.Value.StartDate.Should().Be(newProjectDto.StartDate);
        result.Value.FinishDate.Should().Be(newProjectDto.FinishDate);
        result.Value.Owner.Should().Be(newProjectDto.Owner);
        result.Value.CreateDate.Should().BeSameDateAs(DateTime.Today);
        result.Value.Id.Should().NotBe(default(int));
    }

    [Fact(DisplayName = "RemoveAsync quando executado com sucesso deve remover um projeto se não houver tarefas pendentes e retorná-lo como resultado.")]
    [Trait("ProjectService.RemoveAsync", "Unit")]
    public async System.Threading.Tasks.Task GivenAProjectId_WhenSuccessRemoveAsyncCalled_ThenReturnAProjectDtoDeleted()
    {
        //Arrange
        var userRepoMock = Substitute.For<IUserRepository>();
        var taskRepoMock = Substitute.For<ITaskRepository>();
        var projectRepoMock = Substitute.For<IProjectRepository>();

        var projectIdFake = new Faker("pt_BR").Random.Int(0);

        var projectToDelete = new Faker<Project>("pt_BR")
            .RuleFor(p => p.Id, projectIdFake)
            .RuleFor(p => p.Name, f => f.Lorem.Word())
            .RuleFor(p => p.Owner, f => f.Internet.UserName())
            .RuleFor(p => p.CreateDate, f => f.Date.Past())
            .RuleFor(p => p.StartDate, f => f.Date.Past())
            .RuleFor(p => p.FinishDate, f => f.Date.Past()).Generate();

        var lstTasksFake = new Faker<DomainCore.Task>()
            .RuleFor(p => p.Id, f => f.Random.Int(0))
            .RuleFor(p => p.User, f => f.Internet.UserName())
            .RuleFor(p => p.Title, f => f.Lorem.Word())
            .RuleFor(p => p.Description, f => f.Lorem.Text())
            .RuleFor(p => p.State, (byte)TaskStateEnum.Closed)
            .RuleFor(p => p.Priority, f => (byte)f.Random.Int(1, 3))
            .RuleFor(p => p.TargetDate, f => f.Date.Past())
            .RuleFor(p => p.ProjectId, projectIdFake)
        .Generate(20);

        taskRepoMock.GetAllByProject(Arg.Any<int>()).Returns<List<DomainCore.Task>>(lstTasksFake);
        projectRepoMock.GetAsync(Arg.Any<int>()).Returns<Project?>(projectToDelete);
        projectRepoMock.RemoveAsync(Arg.Any<int>()).Returns<int>(1);

        var projectService = new ProjectService(projectRepoMock, _mapper, taskRepoMock, userRepoMock);

        //Act
        var result = await projectService.RemoveAsync(projectIdFake);

        //Assert
        Assert.IsType<Result<ProjectDTO>>(result);
        Assert.True(result.IsSuccess);
        result.Value.Id.Should().Be(projectToDelete.Id);
        result.Value.Owner.Should().Be(projectToDelete.Owner);
        result.Value.CreateDate.Should().Be(projectToDelete.CreateDate);
        result.Value.StartDate.Should().Be(projectToDelete.StartDate);
        result.Value.FinishDate.Should().Be(projectToDelete.FinishDate);
    }

    [Fact(DisplayName = "RemoveAsync quando executado e um projeto tiver tarefas pendentes, deve retornar falha e uma mensagem informando o número de tarefas não concluídas.")]
    [Trait("ProjectService.RemoveAsync", "Unit")]
    public async System.Threading.Tasks.Task GivenAProjectIdWithTasksNotCompleted_WhenRemoveAsyncCalled_ThenReturnAFailWithMessage()
    {
        //Arrange
        var userRepoMock = Substitute.For<IUserRepository>();
        var taskRepoMock = Substitute.For<ITaskRepository>();
        var projectRepoMock = Substitute.For<IProjectRepository>();

        var projectIdFake = new Faker("pt_BR").Random.Int(0);

        var projectToDelete = new Faker<Project>("pt_BR")
            .RuleFor(p => p.Id, projectIdFake)
            .RuleFor(p => p.Name, f => f.Lorem.Word())
            .RuleFor(p => p.Owner, f => f.Internet.UserName())
            .RuleFor(p => p.CreateDate, f => f.Date.Past())
            .RuleFor(p => p.StartDate, f => f.Date.Past())
            .RuleFor(p => p.FinishDate, f => f.Date.Past()).Generate();

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
        projectRepoMock.GetAsync(Arg.Any<int>()).Returns<Project?>(projectToDelete);
        var msgTasksNotCompleted = $"O projeto possui {lstTasksFake.Count(p => p.State != (byte)TaskStateEnum.Closed)} tarefas não concluídas. É necessário concluí-las para remover o projeto.";
        var projectService = new ProjectService(projectRepoMock, _mapper, taskRepoMock, userRepoMock);

        //Act
        var result = await projectService.RemoveAsync(projectIdFake);

        //Assert
        Assert.IsType<Result<ProjectDTO>>(result);
        Assert.True(result.IsFailed);
        Assert.Equal(msgTasksNotCompleted, result.Reasons[0].Message);
    }
}