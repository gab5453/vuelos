namespace Microservicio.Vuelos.DataManagement.Models.Aeropuerto;

public class AeropuertoFiltroDataModel
{
    public string? Estado { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
