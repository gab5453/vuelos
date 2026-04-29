namespace Microservicio.Vuelos.DataAccess.Entities;

public class EquipajeEntity
{
    public int Id_equipaje { get; set; }
    public int Id_boleto { get; set; }
    public string Tipo_equipaje { get; set; } = string.Empty;
    public decimal Peso_kg { get; set; }

    public virtual BoletoEntity Boleto { get; set; } = null!;
}
