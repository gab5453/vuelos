using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class ClienteDataMapper
{
    public static ClienteDataModel ToDataModel(ClienteEntity entity)
    {
        return new ClienteDataModel
        {
            IdCliente = entity.IdCliente,
            GuidCliente = entity.GuidCliente,
            IdUsuario = entity.IdUsuario,
            Nombres = entity.Nombres,
            Apellidos = entity.Apellidos,
            RazonSocial = entity.RazonSocial,
            TipoIdentificacion = entity.TipoIdentificacion,
            NumeroIdentificacion = entity.NumeroIdentificacion,
            Correo = entity.Correo,
            Telefono = entity.Telefono,
            Direccion = entity.Direccion,
            IdCiudadResidencia = entity.IdCiudadResidencia,
            IdPaisNacionalidad = entity.IdPaisNacionalidad,
            FechaNacimiento = entity.FechaNacimiento,
            Nacionalidad = entity.Nacionalidad,
            Genero = entity.Genero,
            Estado = entity.Estado,
            EsEliminado = entity.EsEliminado,
            CreadoPorUsuario = entity.CreadoPorUsuario,
            FechaRegistroUtc = entity.FechaRegistroUtc,
            ModificadoPorUsuario = entity.ModificadoPorUsuario,
            FechaModificacionUtc = entity.FechaModificacionUtc,
            ModificacionIp = entity.ModificacionIp,
            ServicioOrigen = entity.ServicioOrigen
        };
    }

    public static ClienteEntity ToEntity(ClienteDataModel model)
    {
        return new ClienteEntity
        {
            IdCliente = model.IdCliente,
            GuidCliente = model.GuidCliente,
            IdUsuario = model.IdUsuario,
            Nombres = model.Nombres,
            Apellidos = model.Apellidos,
            RazonSocial = model.RazonSocial,
            TipoIdentificacion = model.TipoIdentificacion,
            NumeroIdentificacion = model.NumeroIdentificacion,
            Correo = model.Correo,
            Telefono = model.Telefono,
            Direccion = model.Direccion,
            IdCiudadResidencia = model.IdCiudadResidencia,
            IdPaisNacionalidad = model.IdPaisNacionalidad,
            FechaNacimiento = model.FechaNacimiento,
            Nacionalidad = model.Nacionalidad,
            Genero = model.Genero,
            Estado = model.Estado,
            EsEliminado = model.EsEliminado,
            CreadoPorUsuario = model.CreadoPorUsuario,
            FechaRegistroUtc = model.FechaRegistroUtc,
            ModificadoPorUsuario = model.ModificadoPorUsuario,
            FechaModificacionUtc = model.FechaModificacionUtc,
            ModificacionIp = model.ModificacionIp,
            ServicioOrigen = model.ServicioOrigen
        };
    }

    public static IReadOnlyList<ClienteDataModel> ToDataModelList(
        IEnumerable<ClienteEntity> entities)
    {
        return entities
            .Select(ToDataModel)
            .ToList();
    }
}
