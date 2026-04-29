using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models.Aeropuerto;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class AeropuertoDataMapper
{
    public static AeropuertoDataModel ToDataModel(this AeropuertoEntity entity)
    {
        return new AeropuertoDataModel
        {
            IdAeropuerto = entity.Id_aeropuerto,
            CodigoIata = entity.Codigo_iata,
            Nombre = entity.Nombre,
            ZonaHoraria = entity.Zona_horaria,
            Latitud = entity.Latitud,
            Longitud = entity.Longitud,
            Estado = entity.Estado
        };
    }
}
