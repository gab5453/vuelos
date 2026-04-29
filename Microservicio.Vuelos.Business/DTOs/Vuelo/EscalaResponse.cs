namespace Microservicio.Vuelos.Business.DTOs.Vuelo;

public class EscalaResponse
{
    public int IdEscala { get; set; }
    public int IdVuelo { get; set; }
    public int IdAeropuerto { get; set; }
    public int Orden { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public DateTime FechaHoraSalida { get; set; }
    public int DuracionMin { get; set; }
    public string TipoEscala { get; set; } = string.Empty;
    public string? Terminal { get; set; }
    public string? Puerta { get; set; }
}
