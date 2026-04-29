namespace Microservicio.Vuelos.DataAccess.Entities;

public class PasajeroEntity
{
    public int Id_pasajero { get; set; }
    public int Id_cliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Documento_identidad { get; set; } = string.Empty;

    public virtual ClienteEntity Cliente { get; set; } = null!;
}
