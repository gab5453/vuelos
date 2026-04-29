using Microservicio.Vuelos.Business.DTOs.Boleto;
using Microservicio.Vuelos.DataManagement.Models.Boleto;

namespace Microservicio.Vuelos.Business.Mappers;

public static class BoletoBusinessMapper
{
    public static BoletoResponse ToResponse(this BoletoDataModel model)
    {
        return new BoletoResponse
        {
            IdBoleto = model.IdBoleto,
            CodigoBoleto = model.CodigoBoleto,
            IdReserva = model.IdReserva,
            IdVuelo = model.IdVuelo,
            IdAsiento = model.IdAsiento,
            IdFactura = model.IdFactura,
            Clase = model.Clase,
            PrecioVueloBase = model.PrecioVueloBase,
            PrecioAsientoExtra = model.PrecioAsientoExtra,
            ImpuestosBoleto = model.ImpuestosBoleto,
            CargoEquipaje = model.CargoEquipaje,
            PrecioFinal = model.PrecioFinal,
            EstadoBoleto = model.EstadoBoleto,
            FechaEmision = model.FechaEmision
        };
    }
}
