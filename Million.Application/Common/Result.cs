using Million.Domain.Common;

namespace Million.Application.Common
{
    /// <summary>
    /// Resultado explícito con éxito/fracaso para Commands/Queries.
    /// Evita excepciones para flujo normal y hace el código más expresivo.
    /// </summary>
    public sealed class Result<T>
    {
        /// <summary>Indica si la operación fue exitosa.</summary>
        public bool IsSuccess { get; }

        /// <summary>Valor devuelto cuando IsSuccess = true.</summary>
        public T? Value { get; }

        /// <summary>Error asociado cuando IsSuccess = false.</summary>
        public Error? Error { get; }

        private Result(bool isSuccess, T? value, Error? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        /// <summary>Crea un resultado exitoso con el valor dado.</summary>
        public static Result<T> Success(T value) => new(true, value, null);

        /// <summary>Crea un resultado fallido con el error indicado.</summary>
        public static Result<T> Failure(Error error) => new(false, default, error);
    }

    /// <summary>
    /// Atajos estáticos para no repetir el genérico al crear resultados.
    /// </summary>
    public static class Result
    {
        public static Result<T> Success<T>(T value) => Result<T>.Success(value);
        public static Result<T> Failure<T>(Million.Domain.Common.Error error) => Result<T>.Failure(error);
    }
}
