using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Application;

namespace TaskManager.Api.Core
{
    /// <summary>
    /// Classe base para o controller
    /// </summary>
    [ApiVersion("1.0")]
    [Produces("application/json", new[] { "text/json" })]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Construtor sem parametro
        /// </summary>
        public BaseController() { }

        protected IActionResult AnswerOk<T>(T? resposta = null, string mensagem = "Sucesso.", IDictionary<string, string[]>? detalhes = null) where T : class
            => Ok(AnswerAPI<T>.Success(resposta, mensagem, detalhes));

        protected IActionResult AnswerBadRequest<T>(T? resposta = null, string mensagem = "Falha.", IDictionary<string, string[]>? detalhes = null) where T : class
            => BadRequest(AnswerAPI<T>.BadRequest(resposta, mensagem, detalhes));

        protected IActionResult AnswerInternalServerError<T>(T? resposta = null, string mensagem = "Erro fatal.", IDictionary<string, string[]>? detalhes = null) where T : class
            => StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), AnswerAPI<T>.InternalServerError(resposta, mensagem, detalhes));

        protected IActionResult AnswerCreated<T>(T? resposta = null, string mensagem = "Recurso criado.", IDictionary<string, string[]>? detalhes = null) where T : class
            => StatusCode(HttpStatusCode.Created.GetHashCode(), AnswerAPI<T>.Created(resposta, mensagem, detalhes));

        protected IActionResult AnswerUnauthorized<T>(T? resposta = null, string mensagem = "Acesso negado.", IDictionary<string, string[]>? detalhes = null) where T : class
            => Unauthorized(AnswerAPI<T>.Unauthorized(resposta, mensagem, detalhes));

        protected IActionResult AnswerConflict<T>(T? resposta = null, string mensagem = "Conflito.", IDictionary<string, string[]>? detalhes = null) where T : class
            => Conflict(AnswerAPI<T>.Conflict(resposta, mensagem, detalhes));

        protected IActionResult AnswerUnprocessableEntity<T>(T? resposta = null, string mensagem = "Não foi possível processar a entidade.", IDictionary<string, string[]>? detalhes = null) where T : class
            => UnprocessableEntity(AnswerAPI<T>.UnprocessableEntity(resposta, mensagem, detalhes));

        protected IActionResult AnswerNotFound<T>(T? resposta = null, string mensagem = "Não encontrado.", IDictionary<string, string[]>? detalhes = null) where T : class
            => NotFound(AnswerAPI<T>.NotFound(resposta, mensagem, detalhes));

        protected IActionResult HandleFailResult<T>(Result<T> result)
        {
            var erroType = result.Errors.FirstOrDefault()?.GetType();
            var erroMsg = result.Errors.FirstOrDefault()?.Message ?? string.Empty;

            return erroType switch
            {
                Type _ when erroType == typeof(FailResult.Unauthorized) => AnswerUnauthorized(string.Empty, erroMsg),
                Type _ when erroType == typeof(FailResult.Conflict) => AnswerConflict(string.Empty, erroMsg),
                Type _ when erroType == typeof(FailResult.InvalidData) => AnswerBadRequest(string.Empty, erroMsg),
                Type _ when erroType == typeof(FailResult.UnprocessableEntity) => AnswerUnprocessableEntity(string.Empty, erroMsg),
                Type _ when erroType == typeof(FailResult.NotFound) => AnswerNotFound(string.Empty, erroMsg),
                _ => AnswerInternalServerError(string.Empty, erroMsg)
            };
        }
    }
}
