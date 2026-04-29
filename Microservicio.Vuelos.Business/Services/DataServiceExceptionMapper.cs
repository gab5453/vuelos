using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Services;

/// <summary>
/// Traduce excepciones de la capa de datos (mensajes conocidos) a excepciones de negocio.
/// </summary>
internal static class DataServiceExceptionMapper
{
    public static void PropagarSiInvalidOperation(InvalidOperationException ex)
    {
        if (ex.Message.Contains("No se encontró", StringComparison.OrdinalIgnoreCase))
            throw new NotFoundException(ex.Message);

        throw new ValidationException(ex.Message);
    }
}
