using FluentResults;

namespace TaskManager.Application
{
    public static class FailResult
    {
        public class Failure : Error
        {
            public Failure(string msg) : base(msg)
            {
            }
        }

        public class Validation : Error
        {
            public Validation(string msg) : base(msg)
            {
            }
        }

        public class Conflict : Error
        {
            public Conflict(string msg) : base(msg)
            {
            }
        }

        public class NotFound : Error
        {
            public NotFound(string msg) : base(msg)
            {
            }
        }

        public class Unauthorized : Error
        {
            public Unauthorized(string msg) : base(msg)
            {
            }
        }

        public class InvalidData : Error
        {
            public InvalidData(string msg) : base(msg)
            {
            }
        }

        public class UnprocessableEntity : Error
        {
            public UnprocessableEntity(string msg) : base(msg)
            {
            }
        }

        public class Unexpected : Error
        {
            public Unexpected(Exception ex, string? info)
            {
                var msg = ex.Message ?? ex.InnerException?.Message ?? ex.InnerException?.InnerException?.Message;
                var stackTrace = ex?.StackTrace ?? ex?.InnerException?.StackTrace;

                if (info is null)
                    Message = $"Erro inesperado: {msg}\r\n{stackTrace}";
                else
                    Message = info;
            }
        }

        public static Failure Falha(string msg = "Falha.")
            => new(msg);

        public static Validation Validacao(string msg = "Falha ao validar.")
            => new(msg);

        public static Conflict Conflito(string msg = "Conflito.")
            => new(msg);

        public static NotFound NaoEncontrado(string msg = "Não encontrado.")
            => new(msg);

        public static Unauthorized NaoAutorizado(string msg = "Não autorizado.")
            => new(msg);

        public static InvalidData DadoInvalido(string msg = "Dado inválido.")
            => new(msg);
        public static UnprocessableEntity EntidadeNaoProcessavel(string msg = "Não foi possível processar a entidade.")
            => new(msg);

        public static Unexpected Inesperado(Exception ex, string? msg = null)
            => new(ex, msg);
    }
}
