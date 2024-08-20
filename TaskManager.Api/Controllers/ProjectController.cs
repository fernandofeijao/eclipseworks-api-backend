using System.Net;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Core;
using TaskManager.Application;

namespace TaskManager.Api
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("getall/{user}")]
        public async Task<IActionResult> GetAll(string user)
        {
            return (await _projectService.GetAllAsync(user)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(NewProjectDTO project)
        {
            return (await _projectService.AddAsync(project)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return (await _projectService.RemoveAsync(id)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }
    }
}
