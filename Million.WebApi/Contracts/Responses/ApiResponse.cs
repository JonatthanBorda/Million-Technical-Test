namespace Million.WebApi.Contracts.Responses
{
    /// <summary>
    /// Estructura para respuestas consistentes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ApiResponse<T>
    {
        public T? Data { get; init; }
        public object? Meta { get; init; }

        public static ApiResponse<T> Ok(T data, object? meta = null) => new() { Data = data, Meta = meta };
    }
}
