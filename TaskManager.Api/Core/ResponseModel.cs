using System.Net;

namespace TaskManager.Api
{
    public class StatusResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public IDictionary<string, string[]> Details { get; set; } = new Dictionary<string, string[]>();
    }

    public class ApiResponseModel<T>
    {
        public T? Response { get; set; }
        public StatusResponseModel Status { get; set; }
    }

    public static class AnswerAPI<T> where T : class
    {
        private static ApiResponseModel<T> ResultModel(string message, IDictionary<string, string[]>? details, int code, T? result = null) =>
            new()
            {
                Response = result,
                Status = new StatusResponseModel
                {
                    Code = code,
                    Message = message,
                    Details = details ?? new Dictionary<string, string[]>()
                }
            };

        public static ApiResponseModel<T> Success(T? resposta = null, string mensagem = "Sucesso.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.OK.GetHashCode(), resposta);

        public static ApiResponseModel<T> BadRequest(T? resposta = null, string mensagem = "Falha.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.BadRequest.GetHashCode(), resposta);

        public static ApiResponseModel<T> InternalServerError(T? resposta = null, string mensagem = "Erro fatal.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.InternalServerError.GetHashCode(), resposta);

        public static ApiResponseModel<T> Created(T? resposta = null, string mensagem = "Recurso criado.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.Created.GetHashCode(), resposta);

        public static ApiResponseModel<T> Unauthorized(T? resposta = null, string mensagem = "Acesso negado.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.Unauthorized.GetHashCode(), resposta);

        public static ApiResponseModel<T> Conflict(T? resposta = null, string mensagem = "Conflito.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.Conflict.GetHashCode(), resposta);

        public static ApiResponseModel<T> UnprocessableEntity(T? resposta = null, string mensagem = "Não foi possível processar a entidade.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.UnprocessableEntity.GetHashCode(), resposta);

        public static ApiResponseModel<T> NotFound(T? resposta = null, string mensagem = "Não encontrado.", IDictionary<string, string[]>? detalhes = null)
            => ResultModel(mensagem, detalhes, HttpStatusCode.NotFound.GetHashCode(), resposta);
    }

}
