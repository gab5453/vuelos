namespace Microservicio.Vuelos.Business.DTOs.Aeropuerto;

public class AeropuertoFiltroRequest
{
    public string? Estado { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
