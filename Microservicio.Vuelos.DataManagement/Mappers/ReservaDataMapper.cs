namespace Microservicio.Vuelos.DataManagement.Mappers;

using Microservicio.Vuelos.DataManagement.Models.Reserva;
using Microservicio.Vuelos.DataAccess.Entities;

public static class ReservaDataMapper
{
    public static ReservaDataModel ToDataModel(this ReservaEntity entity)
    {
        return new ReservaDataModel
        {
            IdReserva = entity.Id_reserva,
            GuidReserva = entity.Guid_reserva,
            CodigoReserva = entity.Codigo_reserva,
            IdCliente = entity.Id_cliente,
            IdPasajero = entity.Id_pasajero,
            IdVuelo = entity.Id_vuelo,
            IdAsiento = entity.Id_asiento,
            FechaReservaUtc = entity.Fecha_reserva_utc,
            FechaInicio = entity.Fecha_inicio,
            FechaFin = entity.Fecha_fin,
            SubtotalReserva = entity.Subtotal_reserva,
            ValorIva = entity.Valor_iva,
            TotalReserva = entity.Total_reserva,
            OrigenCanalReserva = entity.Origen_canal_reserva,
            EstadoReserva = entity.Estado_reserva,
            ContactoEmail = entity.Contacto_email,
            ContactoTelefono = entity.Contacto_telefono,
            Observaciones = entity.Observaciones
        };
    }
}
