using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models.Pasajero;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class PasajeroDataMapper
{
    public static PasajeroDataModel ToDataModel(this PasajeroEntity entity)
    {
        return new PasajeroDataModel
        {
            IdPasajero = entity.Id_pasajero,
            NombrePasajero = entity.Nombre,
            ApellidoPasajero = entity.Apellido,
            NumeroDocumentoPasajero = entity.Documento_identidad,
            IdCliente = entity.Id_cliente
        };
    }

    public static PasajeroEntity ToEntity(this PasajeroDataModel model)
    {
        return new PasajeroEntity
        {
            Id_pasajero = model.IdPasajero,
            Nombre = model.NombrePasajero,
            Apellido = model.ApellidoPasajero,
            Documento_identidad = model.NumeroDocumentoPasajero,
            Id_cliente = model.IdCliente ?? 0
        };
    }
}
