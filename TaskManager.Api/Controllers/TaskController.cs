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
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("getall/{projectId}")]
        public async Task<IActionResult> GetAll(int projectId)
        {
            return (await _taskService.GetAllByProjectAsync(projectId)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(NewTaskDTO taskDto)
        {
            return (await _taskService.AddAsync(taskDto)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }

        [HttpPost("comment")]
        public async Task<IActionResult> Comment(NewTaskDiscussionDTO taskDiscussionDto)
        {
            var keyValuePair = Request.Headers.FirstOrDefault(h => h.Key == "User").Value;
            if (keyValuePair.Count > 0)
                taskDiscussionDto.ActionUser = keyValuePair[0];

            return (await _taskService.CommentAsync(taskDiscussionDto)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit(EditTaskDTO taskDto)
        {
            var keyValuePair = Request.Headers.FirstOrDefault(h => h.Key == "User").Value;
            if (keyValuePair.Count > 0)
                taskDto.ActionUser = keyValuePair[0];

            return (await _taskService.UpdateAsync(taskDto)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }
    }
}
