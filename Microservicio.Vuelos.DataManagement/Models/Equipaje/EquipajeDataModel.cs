namespace Microservicio.Vuelos.DataManagement.Models.Equipaje;

public class EquipajeDataModel
{
    public int IdEquipaje { get; set; }
    public int IdBoleto { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal PesoKg { get; set; }
}
