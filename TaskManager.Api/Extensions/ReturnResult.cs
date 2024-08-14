using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api
{
    public static class ResultadoRetorno
    {
        public static IActionResult Answer<T>(this Result<T> result, Func<Result<T>, IActionResult> success, Func<Result<T>, IActionResult> fail)
            => result.IsSuccess ? success(result) : fail(result);
    }
}
