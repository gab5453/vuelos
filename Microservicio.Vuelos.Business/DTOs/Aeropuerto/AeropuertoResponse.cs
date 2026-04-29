namespace Microservicio.Vuelos.Business.DTOs.Aeropuerto;

public class AeropuertoResponse
{
    public int IdAeropuerto { get; set; }
    public string CodigoIata { get; set; } = string.Empty;
    public string? CodigoIcao { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ZonaHoraria { get; set; } = string.Empty;
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public string Estado { get; set; } = string.Empty;
}
