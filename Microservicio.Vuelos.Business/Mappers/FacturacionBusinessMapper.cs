using Microservicio.Vuelos.Business.DTOs.Facturacion;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class FacturacionBusinessMapper
{
    public static FacturacionDataModel ToDataModel(CrearFacturacionRequest request)
    {
        return new FacturacionDataModel
        {
            IdCliente = request.IdCliente,
            IdReserva = request.IdReserva,
            IdMetodo = request.IdMetodo,
            Subtotal = request.Subtotal,
            ValorIva = request.ValorIva,
            CargoServicio = request.CargoServicio,
            Total = request.Total,
            ObservacionesFactura = request.ObservacionesFactura,

            Estado = "ABI",
            FechaRegistroUtc = DateTimeOffset.UtcNow,
            CreadoPorUsuario = request.CreadoPorUsuario ?? "sistema",
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen ?? "Microservicio.Vuelos.Api"
        };
    }

    public static FacturacionDataModel ToDataModel(ActualizarFacturacionRequest request, FacturacionDataModel existingModel)
    {
        existingModel.Subtotal = request.Subtotal;
        existingModel.ValorIva = request.ValorIva;
        existingModel.Total = request.Total;
        existingModel.ObservacionesFactura = request.ObservacionesFactura;

        existingModel.FechaModificacionUtc = DateTimeOffset.UtcNow;
        existingModel.ModificadoPorUsuario = request.ModificadoPorUsuario;
        existingModel.ModificacionIp = request.ModificacionIp;
        existingModel.ServicioOrigen = request.ServicioOrigen ?? "Microservicio.Vuelos.Api";

        return existingModel;
    }

    public static FacturacionResponse ToResponse(FacturacionDataModel model)
    {
        return new FacturacionResponse
        {
            IdFactura = model.IdFactura,
            GuidFactura = model.GuidFactura,
            IdCliente = model.IdCliente,
            IdReserva = model.IdReserva,
            IdMetodo = model.IdMetodo,
            NumeroFactura = model.NumeroFactura,
            FechaEmision = model.FechaEmision,
            Subtotal = model.Subtotal,
            ValorIva = model.ValorIva,
            CargoServicio = model.CargoServicio,
            Total = model.Total,
            ObservacionesFactura = model.ObservacionesFactura,
            Estado = model.Estado
        };
    }
}
