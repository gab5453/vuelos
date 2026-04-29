namespace Microservicio.Vuelos.Business.DTOs.Facturacion;

public class FacturacionResponse
{
    public int IdFactura { get; set; }
    public Guid GuidFactura { get; set; }
    public int IdCliente { get; set; }
    public int IdReserva { get; set; }
    public int IdMetodo { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal CargoServicio { get; set; }
    public decimal Total { get; set; }
    public string? ObservacionesFactura { get; set; }
    public string Estado { get; set; } = string.Empty;
}
