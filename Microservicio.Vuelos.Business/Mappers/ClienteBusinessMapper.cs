using Microservicio.Vuelos.Business.DTOs.Cliente;
using Microservicio.Vuelos.DataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservicio.Vuelos.Business.Mappers;

public static class ClienteBusinessMapper
{
    public static ClienteDataModel ToDataModel(CrearClienteRequest request)
    {
        return new ClienteDataModel
        {
            IdUsuario = request.IdUsuario,
            Nombres = request.Nombres?.Trim(),
            Apellidos = request.Apellidos?.Trim(),
            RazonSocial = request.RazonSocial?.Trim(),
            TipoIdentificacion = request.TipoIdentificacion.ToUpper().Trim(),
            NumeroIdentificacion = request.NumeroIdentificacion.Trim(),
            Correo = request.Correo.ToLower().Trim(),
            Telefono = request.Telefono?.Trim(),
            Direccion = request.Direccion?.Trim(),
            IdCiudadResidencia = request.IdCiudadResidencia,
            IdPaisNacionalidad = request.IdPaisNacionalidad,
            FechaNacimiento = request.FechaNacimiento,
            Nacionalidad = request.Nacionalidad?.Trim(),
            Genero = request.Genero?.Trim(),
            Estado = "ACT",
            EsEliminado = false,
            CreadoPorUsuario = request.CreadoPorUsuario,
            FechaRegistroUtc = DateTimeOffset.UtcNow,
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen
        };
    }

    public static ClienteDataModel ToDataModel(ActualizarClienteRequest request)
    {
        return new ClienteDataModel
        {
            GuidCliente = request.GuidCliente,
            Nombres = request.Nombres?.Trim(),
            Apellidos = request.Apellidos?.Trim(),
            RazonSocial = request.RazonSocial?.Trim(),
            TipoIdentificacion = request.TipoIdentificacion.ToUpper().Trim(),
            NumeroIdentificacion = request.NumeroIdentificacion.Trim(),
            Correo = request.Correo.ToLower().Trim(),
            Telefono = request.Telefono?.Trim(),
            Direccion = request.Direccion?.Trim(),
            IdCiudadResidencia = request.IdCiudadResidencia,
            IdPaisNacionalidad = request.IdPaisNacionalidad,
            FechaNacimiento = request.FechaNacimiento,
            Nacionalidad = request.Nacionalidad?.Trim(),
            Genero = request.Genero?.Trim(),
            Estado = request.Estado.ToUpper().Trim(),
            ModificadoPorUsuario = request.ModificadoPorUsuario,
            FechaModificacionUtc = DateTimeOffset.UtcNow,
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen
        };
    }

    public static ClienteFiltroDataModel ToFiltroDataModel(ClienteFiltroRequest request)
    {
        return new ClienteFiltroDataModel
        {
            Correo = request.Correo?.ToLower().Trim(),
            NumeroIdentificacion = request.NumeroIdentificacion?.Trim(),
            Estado = request.Estado?.ToUpper().Trim(),
            PaginaActual = request.Page,
            TamanioPagina = request.PageSize
        };
    }

    public static ClienteResponse ToResponse(ClienteDataModel model)
    {
        return new ClienteResponse
        {
            IdCliente = model.IdCliente,
            GuidCliente = model.GuidCliente,
            Nombres = model.Nombres,
            Apellidos = model.Apellidos,
            RazonSocial = model.RazonSocial,
            TipoIdentificacion = model.TipoIdentificacion,
            NumeroIdentificacion = model.NumeroIdentificacion,
            Correo = model.Correo,
            Telefono = model.Telefono,
            Direccion = model.Direccion,
            FechaNacimiento = model.FechaNacimiento,
            Nacionalidad = model.Nacionalidad,
            Genero = model.Genero,
            Estado = model.Estado,
            CreadoPorUsuario = model.CreadoPorUsuario,
            FechaRegistroUtc = model.FechaRegistroUtc,
            ModificadoPorUsuario = model.ModificadoPorUsuario,
            FechaModificacionUtc = model.FechaModificacionUtc
        };
    }

    public static IReadOnlyList<ClienteResponse> ToResponseList(
        IEnumerable<ClienteDataModel> models)
    {
        return models
            .Select(ToResponse)
            .ToList();
    }
}
