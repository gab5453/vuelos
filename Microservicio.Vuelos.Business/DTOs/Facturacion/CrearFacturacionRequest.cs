namespace Microservicio.Vuelos.Business.DTOs.Facturacion;

/// <summary>
/// Modelo de datos (DTO) para la solicitud de creación de una nueva factura.
/// Define los datos obligatorios requeridos desde el exterior.
/// </summary>
public class CrearFacturacionRequest
{
    public int IdCliente { get; set; }
    public int IdReserva { get; set; }
    public int IdMetodo { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal CargoServicio { get; set; }
    public decimal Total { get; set; }
    public string? ObservacionesFactura { get; set; }
    
    // Auditoría (Opcionales para el contrato, pero mantenidos por estructura interna)
    public string? CreadoPorUsuario { get; set; }
    public string? ServicioOrigen { get; set; }
    public string? ModificacionIp { get; set; }
}
