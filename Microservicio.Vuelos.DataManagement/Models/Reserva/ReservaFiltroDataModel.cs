namespace Microservicio.Vuelos.DataManagement.Models.Reserva;

public class ReservaFiltroDataModel
{
    public int? IdCliente { get; set; }
    public string? Estado { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
