using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models.Equipaje;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class EquipajeDataMapper
{
    public static EquipajeDataModel ToDataModel(this EquipajeEntity entity)
    {
        return new EquipajeDataModel
        {
            IdEquipaje = entity.Id_equipaje,
            IdBoleto = entity.Id_boleto,
            Tipo = entity.Tipo_equipaje,
            PesoKg = entity.Peso_kg
        };
    }

    public static EquipajeEntity ToEntity(this EquipajeDataModel model)
    {
        return new EquipajeEntity
        {
            Id_equipaje = model.IdEquipaje,
            Id_boleto = model.IdBoleto,
            Tipo_equipaje = model.Tipo,
            Peso_kg = model.PesoKg
        };
    }
}
