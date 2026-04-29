namespace Microservicio.Vuelos.DataManagement.Mappers;

using Microservicio.Vuelos.DataManagement.Models.Vuelo;
using Microservicio.Vuelos.DataAccess.Entities;

public static class VueloDataMapper
{
    public static VueloDataModel ToDataModel(this VueloEntity entity)
    {
        return new VueloDataModel
        {
            IdVuelo = entity.Id_vuelo,
            NumeroVuelo = entity.Numero_vuelo,
            FechaHoraSalida = entity.Fecha_hora_salida,
            FechaHoraLlegada = entity.Fecha_hora_llegada,
            DuracionMin = entity.Duracion_min,
            PrecioBase = entity.Precio_base,
            CapacidadTotal = entity.Capacidad_total,
            EstadoVuelo = entity.Estado_vuelo,
            Estado = entity.Estado,
            Origen = entity.AeropuertoOrigen?.ToInfoDataModel()!,
            Destino = entity.AeropuertoDestino?.ToInfoDataModel()!
        };
    }

    private static AeropuertoInfoDataModel ToInfoDataModel(this AeropuertoEntity entity)
    {
        return new AeropuertoInfoDataModel
        {
            IdAeropuerto = entity.Id_aeropuerto,
            CodigoIata = entity.Codigo_iata,
            Nombre = entity.Nombre,
            NombreCiudad = entity.Ciudad?.Nombre ?? string.Empty
        };
    }

    public static EscalaDataModel ToDataModel(this EscalaEntity entity)
    {
        return new EscalaDataModel
        {
            IdEscala = entity.Id_escala,
            IdVuelo = entity.Id_vuelo,
            IdAeropuerto = entity.Id_aeropuerto_escala,
            Orden = entity.Orden
            // NOTE: Missing fields in entity: FechaHoraLlegada, DuracionMin, etc.
        };
    }

    public static AsientoDataModel ToDataModel(this AsientoEntity entity)
    {
        return new AsientoDataModel
        {
            IdAsiento = entity.Id_asiento,
            IdVuelo = entity.Id_vuelo,
            NumeroAsiento = entity.Numero_asiento,
            Clase = entity.Clase,
            Disponible = entity.Disponible,
            PrecioExtra = entity.Precio_extra
        };
    }
}
