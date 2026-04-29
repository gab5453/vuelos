using Microservicio.Vuelos.Business.DTOs.Facturacion;

namespace Microservicio.Vuelos.Business.Validators;

public static class FacturacionValidator
{
    public static IReadOnlyCollection<string> ValidarCreacion(CrearFacturacionRequest request)
    {
        var errors = new List<string>();

        if (request.IdCliente <= 0)
            errors.Add("El Id del cliente es obligatorio y debe ser mayor a 0.");

        if (request.IdReserva <= 0)
            errors.Add("El Id de la reserva es obligatorio y debe ser mayor a 0.");

        if (request.IdMetodo <= 0)
            errors.Add("El Id del método de pago es obligatorio y debe ser mayor a 0.");

        if (request.Subtotal < 0)
            errors.Add("El subtotal no puede ser negativo.");

        if (request.ValorIva < 0)
            errors.Add("El valor del IVA no puede ser negativo.");

        if (request.Total < 0)
            errors.Add("El total no puede ser negativo.");

        return errors;
    }

    public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarFacturacionRequest request)
    {
        var errors = new List<string>();

        if (request.GuidFactura == Guid.Empty)
            errors.Add("El Guid de la factura es inválido.");

        if (request.Subtotal < 0)
            errors.Add("El subtotal no puede ser negativo.");

        if (request.ValorIva < 0)
            errors.Add("El valor del IVA no puede ser negativo.");

        if (request.Total < 0)
            errors.Add("El total no puede ser negativo.");

        if (string.IsNullOrWhiteSpace(request.ModificadoPorUsuario))
            errors.Add("El usuario modificador es obligatorio.");

        return errors;
    }
}
