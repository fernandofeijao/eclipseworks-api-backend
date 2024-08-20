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
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("task")]
        public async Task<IActionResult> Task(ReportFilterDTO reportDto)
        {
            var keyValuePair = Request.Headers.FirstOrDefault(h => h.Key == "User").Value;
            if (keyValuePair.Count > 0)
                reportDto.ActionUser = keyValuePair[0];

            return (await _reportService.GetTaskByUser(reportDto)).Answer(
                success => AnswerOk(success.Value),
                fail => HandleFailResult(fail)
            );
        }
    }
}
