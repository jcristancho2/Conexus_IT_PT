using System;

namespace InvoicesSystem.API.Models.Responses;

/// <summary>
/// Clase genérica para estandarizar las respuestas de la API
/// </summary>
/// <typeparam name="T">Tipo de datos que contendrá la respuesta</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje descriptivo de la respuesta
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Datos de la respuesta
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Lista de errores (si los hay)
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Timestamp de la respuesta
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Código de estado HTTP
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Constructor privado para forzar el uso de métodos estáticos
    /// </summary>
    private ApiResponse() { }

    /// <summary>
    /// Crea una respuesta exitosa con datos
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = 200,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta exitosa sin datos (para operaciones como DELETE)
    /// </summary>
    public static ApiResponse<T> SuccessResponse(string message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = default,
            StatusCode = 200,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta de error con un mensaje
    /// </summary>
    public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta de error con múltiples errores
    /// </summary>
    public static ApiResponse<T> ErrorResponse(List<string> errors, string message = "Se encontraron errores", int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta de no encontrado (404)
    /// </summary>
    public static ApiResponse<T> NotFoundResponse(string message = "Recurso no encontrado")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            StatusCode = 404,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta de no autorizado (401)
    /// </summary>
    public static ApiResponse<T> UnauthorizedResponse(string message = "No autorizado")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            StatusCode = 401,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta de validación fallida (422)
    /// </summary>
    public static ApiResponse<T> ValidationErrorResponse(List<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Error de validación",
            Data = default,
            Errors = errors,
            StatusCode = 422,
            Timestamp = DateTime.UtcNow
        };
    }
}