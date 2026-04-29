namespace Microservicio.Vuelos.Business.DTOs.Reserva;

public class ActualizarEstadoReservaRequest
{
    public string EstadoReserva { get; set; } = string.Empty;
    public string? MotivoCancelacion { get; set; }
}
