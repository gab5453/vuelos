using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Mappers;

internal static class RowVersionMapper
{
    public static byte[]? DesdeBase64(string? base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            return null;

        try
        {
            return Convert.FromBase64String(base64.Trim());
        }
        catch (FormatException)
        {
            throw new ValidationException("RowVersionBase64 no tiene un formato Base64 válido.");
        }
    }

    public static string? ABase64(byte[]? bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return null;
        return Convert.ToBase64String(bytes);
    }
}
