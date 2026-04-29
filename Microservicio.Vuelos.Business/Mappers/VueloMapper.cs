namespace Microservicio.Vuelos.Business.Mappers;

using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.DataManagement.Models.Vuelo;

public static class VueloMapper
{
    public static VueloFiltroDataModel ToDataModel(this VueloFiltroRequest request)
    {
        return new VueloFiltroDataModel
        {
            IdAeropuertoOrigen = request.IdAeropuertoOrigen,
            IdAeropuertoDestino = request.IdAeropuertoDestino,
            FechaSalida = request.FechaSalida
        };
    }

    public static VueloResponse ToResponse(this VueloDataModel dataModel)
    {
        return new VueloResponse
        {
            IdVuelo = dataModel.IdVuelo,
            NumeroVuelo = dataModel.NumeroVuelo,
            IdAeropuertoOrigen = dataModel.Origen.IdAeropuerto,
            IdAeropuertoDestino = dataModel.Destino.IdAeropuerto,
            FechaHoraSalida = dataModel.FechaHoraSalida,
            FechaHoraLlegada = dataModel.FechaHoraLlegada,
            DuracionMin = dataModel.DuracionMin,
            PrecioBase = dataModel.PrecioBase,
            CapacidadTotal = dataModel.CapacidadTotal,
            EstadoVuelo = dataModel.EstadoVuelo,
            Estado = dataModel.Estado,

            Origen = new AeropuertoInfoResponse
            {
                IdAeropuerto = dataModel.Origen.IdAeropuerto,
                CodigoIata = dataModel.Origen.CodigoIata,
                Nombre = dataModel.Origen.Nombre,
                NombreCiudad = dataModel.Origen.NombreCiudad
            },
            Destino = new AeropuertoInfoResponse
            {
                IdAeropuerto = dataModel.Destino.IdAeropuerto,
                CodigoIata = dataModel.Destino.CodigoIata,
                Nombre = dataModel.Destino.Nombre,
                NombreCiudad = dataModel.Destino.NombreCiudad
            }
        };
    }

    public static EscalaResponse ToResponse(this EscalaDataModel model)
    {
        return new EscalaResponse
        {
            IdEscala = model.IdEscala,
            IdVuelo = model.IdVuelo,
            IdAeropuerto = model.IdAeropuerto,
            Orden = model.Orden,
            FechaHoraLlegada = model.FechaHoraLlegada,
            FechaHoraSalida = model.FechaHoraSalida,
            DuracionMin = model.DuracionMin,
            TipoEscala = model.TipoEscala,
            Terminal = model.Terminal,
            Puerta = model.Puerta
        };
    }

    public static AsientoResponse ToResponse(this AsientoDataModel model)
    {
        return new AsientoResponse
        {
            IdAsiento = model.IdAsiento,
            IdVuelo = model.IdVuelo,
            NumeroAsiento = model.NumeroAsiento,
            Clase = model.Clase,
            Disponible = model.Disponible,
            PrecioExtra = model.PrecioExtra,
            Posicion = model.Posicion
        };
    }
}
