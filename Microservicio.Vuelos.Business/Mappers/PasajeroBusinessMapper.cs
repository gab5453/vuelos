using Microservicio.Vuelos.Business.DTOs.Pasajero;
using Microservicio.Vuelos.DataManagement.Models.Pasajero;

namespace Microservicio.Vuelos.Business.Mappers;

public static class PasajeroBusinessMapper
{
    public static PasajeroDataModel ToDataModel(this PasajeroRequest request)
    {
        return new PasajeroDataModel
        {
            NombrePasajero = request.NombrePasajero,
            ApellidoPasajero = request.ApellidoPasajero,
            TipoDocumentoPasajero = request.TipoDocumentoPasajero,
            NumeroDocumentoPasajero = request.NumeroDocumentoPasajero,
            IdCliente = request.IdCliente,
            FechaNacimientoPasajero = request.FechaNacimientoPasajero,
            NacionalidadPasajero = request.NacionalidadPasajero,
            EmailContactoPasajero = request.EmailContactoPasajero,
            TelefonoContactoPasajero = request.TelefonoContactoPasajero,
            GeneroPasajero = request.GeneroPasajero,
            RequiereAsistencia = request.RequiereAsistencia,
            ObservacionesPasajero = request.ObservacionesPasajero
        };
    }

    public static PasajeroResponse ToResponse(this PasajeroDataModel model)
    {
        return new PasajeroResponse
        {
            IdPasajero = model.IdPasajero,
            NombrePasajero = model.NombrePasajero,
            ApellidoPasajero = model.ApellidoPasajero,
            TipoDocumentoPasajero = model.TipoDocumentoPasajero,
            NumeroDocumentoPasajero = model.NumeroDocumentoPasajero,
            FechaNacimientoPasajero = model.FechaNacimientoPasajero,
            RequiereAsistencia = model.RequiereAsistencia
        };
    }
}
