namespace Microservicio.Vuelos.Api.Models.Common;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
    public PaginationMeta? Meta { get; init; }

    public static ApiResponse<T> Exitoso(T data, string message = "Operación exitosa", PaginationMeta? meta = null) =>
        new() { Success = true, Message = message, Data = data, Meta = meta };

    public static ApiResponse<T> SinContenido(string message = "Sin datos") =>
        new() { Success = true, Message = message, Data = default };

    public static ApiResponse<T> Ok(T data, string message = "Operación exitosa", PaginationMeta? meta = null) =>
        Exitoso(data, message, meta);

    public static ApiResponse<T> Fallido(string message = "Error en la operación") =>
        new() { Success = false, Message = message, Data = default };
}

public class PaginationMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
}
