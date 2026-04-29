namespace Microservicio.Vuelos.Business.DTOs.Reserva;

public class ReservaResponse
{
    public int IdReserva { get; set; }
    public string CodigoReserva { get; set; } = string.Empty;
    public int IdCliente { get; set; }
    public int IdVuelo { get; set; }
    public string EstadoReserva { get; set; } = string.Empty;
    public DateTime FechaReservaUtc { get; set; }
}
