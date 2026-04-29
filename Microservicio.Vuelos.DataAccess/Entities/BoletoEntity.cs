namespace Microservicio.Vuelos.DataAccess.Entities;

public class BoletoEntity
{
    public int Id_boleto { get; set; }
    public string Codigo_boleto { get; set; } = string.Empty;
    public int Id_reserva { get; set; }
    public int Id_vuelo { get; set; }
    public int Id_asiento { get; set; }
    public int Id_factura { get; set; }
    public string Clase { get; set; } = string.Empty;
    public decimal Precio_vuelo_base { get; set; }
    public decimal Precio_asiento_extra { get; set; }
    public decimal Impuestos_boleto { get; set; }
    public decimal Cargo_equipaje { get; set; }
    public decimal Precio_final { get; set; }
    public string Estado_boleto { get; set; } = string.Empty;
    public DateTime Fecha_emision { get; set; }

    public virtual ReservaEntity Reserva { get; set; } = null!;
    public virtual ICollection<EquipajeEntity> Equipajes { get; set; } = new List<EquipajeEntity>();
}
