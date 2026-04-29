namespace Microservicio.Vuelos.DataAccess.Entities;

public class AsientoEntity
{
    public int Id_asiento { get; set; }
    public int Id_vuelo { get; set; }
    public string Numero_asiento { get; set; } = string.Empty;
    public string Clase { get; set; } = string.Empty;
    public decimal Precio_extra { get; set; }
    public bool Disponible { get; set; }

    public virtual VueloEntity Vuelo { get; set; } = null!;
}
