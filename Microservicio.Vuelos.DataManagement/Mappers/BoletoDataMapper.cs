using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models.Boleto;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class BoletoDataMapper
{
    public static BoletoDataModel ToDataModel(this BoletoEntity entity)
    {
        return new BoletoDataModel
        {
            IdBoleto = entity.Id_boleto,
            CodigoBoleto = entity.Codigo_boleto,
            IdReserva = entity.Id_reserva,
            IdVuelo = entity.Id_vuelo,
            IdAsiento = entity.Id_asiento,
            IdFactura = entity.Id_factura,
            Clase = entity.Clase,
            PrecioVueloBase = entity.Precio_vuelo_base,
            PrecioAsientoExtra = entity.Precio_asiento_extra,
            ImpuestosBoleto = entity.Impuestos_boleto,
            CargoEquipaje = entity.Cargo_equipaje,
            PrecioFinal = entity.Precio_final,
            EstadoBoleto = entity.Estado_boleto,
            FechaEmision = entity.Fecha_emision
        };
    }
}
