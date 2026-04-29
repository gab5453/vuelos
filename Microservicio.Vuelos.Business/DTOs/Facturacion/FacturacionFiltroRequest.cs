namespace Microservicio.Vuelos.Business.DTOs.Facturacion;

public class FacturacionFiltroRequest
{
    public string? Estado { get; set; }
    public int? IdCliente { get; set; }
    public string? NumeroFactura { get; set; }
    public DateTime? FechaEmisionInicio { get; set; }
    public DateTime? FechaEmisionFin { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
