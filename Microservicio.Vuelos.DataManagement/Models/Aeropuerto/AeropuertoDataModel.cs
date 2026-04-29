namespace Microservicio.Vuelos.DataManagement.Models.Aeropuerto;

public class AeropuertoDataModel
{
    public int IdAeropuerto { get; set; }
    public string CodigoIata { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string ZonaHoraria { get; set; } = string.Empty;
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}
