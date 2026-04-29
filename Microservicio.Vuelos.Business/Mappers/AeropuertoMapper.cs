using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.DataManagement.Models.Aeropuerto;

namespace Microservicio.Vuelos.Business.Mappers;

public static class AeropuertoMapper
{
    public static AeropuertoResponse ToResponse(this AeropuertoDataModel model)
    {
        return new AeropuertoResponse
        {
            IdAeropuerto = model.IdAeropuerto,
            CodigoIata = model.CodigoIata,
            Nombre = model.Nombre,
            ZonaHoraria = model.ZonaHoraria,
            Latitud = model.Latitud,
            Longitud = model.Longitud,
            Estado = model.Estado
        };
    }
}
