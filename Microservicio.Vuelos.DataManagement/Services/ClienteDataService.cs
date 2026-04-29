using Microservicio.Vuelos.DataAccess.Common;
using Microservicio.Vuelos.DataAccess.Queries;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Vuelos.DataManagement.Services;

public class ClienteDataService : IClienteDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClienteDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClienteDataModel?> ObtenerPorIdAsync(
        int idCliente,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorIdAsync(idCliente, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorGuidAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorIdUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorIdUsuarioAsync(idUsuario, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorCorreoAsync(correo, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorNumeroIdentificacionAsync(
                tipoIdentificacion,
                numeroIdentificacion,
                cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<DataPagedResult<ClienteDataModel>> BuscarAsync(
        ClienteFiltroDataModel filtro,
        CancellationToken cancellationToken = default)
    {
        var termino = new[]
        {
            filtro.Nombres,
            filtro.Apellidos,
            filtro.RazonSocial,
            filtro.Correo,
            filtro.NumeroIdentificacion
        }
        .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t)) ?? string.Empty;

        PagedResult<ClienteResumenDto> resultado;

        if (!string.IsNullOrWhiteSpace(termino))
        {
            resultado = await _unitOfWork.ClienteQueryRepository
                .BuscarClientesAsync(
                    termino,
                    filtro.PaginaActual,
                    filtro.TamanioPagina,
                    cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            resultado = await _unitOfWork.ClienteQueryRepository
                .ListarClientesPorEstadoAsync(
                    filtro.Estado,
                    filtro.PaginaActual,
                    filtro.TamanioPagina,
                    cancellationToken);
        }
        else
        {
            resultado = await _unitOfWork.ClienteQueryRepository
                .ListarClientesAsync(
                    filtro.PaginaActual,
                    filtro.TamanioPagina,
                    cancellationToken);
        }

        return DataPagedResult<ClienteDataModel>.DesdeDal(
            resultado,
            dto => new ClienteDataModel
            {
                IdCliente = dto.IdCliente,
                GuidCliente = dto.GuidCliente,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                RazonSocial = dto.RazonSocial,
                TipoIdentificacion = dto.TipoIdentificacion,
                NumeroIdentificacion = dto.NumeroIdentificacion,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                FechaNacimiento = dto.FechaNacimiento,
                Nacionalidad = dto.Nacionalidad,
                Genero = dto.Genero,
                Estado = dto.Estado
            });
    }

    public async Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ClienteRepository
            .ExisteCorreoAsync(correo, cancellationToken);
    }

    public async Task<bool> ExisteNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ClienteRepository
            .ExisteNumeroIdentificacionAsync(
                tipoIdentificacion,
                numeroIdentificacion,
                cancellationToken);
    }

    public async Task<bool> ExisteUsuarioVinculadoAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ClienteRepository
            .ExisteUsuarioVinculadoAsync(idUsuario, cancellationToken);
    }

    public async Task<ClienteDataModel> CrearAsync(
        ClienteDataModel model,
        CancellationToken cancellationToken = default)
    {
        var entity = ClienteDataMapper.ToEntity(model);

        await _unitOfWork.ClienteRepository
            .AgregarAsync(entity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ActualizarAsync(
        ClienteDataModel model,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(model.GuidCliente, cancellationToken);

        if (entity is null) return null;

        entity.Nombres = model.Nombres;
        entity.Apellidos = model.Apellidos;
        entity.RazonSocial = model.RazonSocial;
        entity.TipoIdentificacion = model.TipoIdentificacion;
        entity.NumeroIdentificacion = model.NumeroIdentificacion;
        entity.Correo = model.Correo;
        entity.Telefono = model.Telefono;
        entity.Direccion = model.Direccion;
        entity.IdCiudadResidencia = model.IdCiudadResidencia;
        entity.IdPaisNacionalidad = model.IdPaisNacionalidad;
        entity.FechaNacimiento = model.FechaNacimiento;
        entity.Nacionalidad = model.Nacionalidad;
        entity.Genero = model.Genero;
        entity.Estado = model.Estado;
        entity.ModificadoPorUsuario = model.ModificadoPorUsuario;
        entity.ModificacionIp = model.ModificacionIp;
        entity.ServicioOrigen = model.ServicioOrigen;

        _unitOfWork.ClienteRepository.Actualizar(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<bool> EliminarLogicoAsync(
        Guid guidCliente,
        string eliminadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        if (entity is null) return false;

        _unitOfWork.ClienteRepository.EliminarLogico(entity);
        entity.ModificadoPorUsuario = eliminadoPorUsuario;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        Guid guidCliente,
        string nuevoEstado,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        if (entity is null) return false;

        _unitOfWork.ClienteRepository.CambiarEstado(entity, nuevoEstado);
        entity.ModificadoPorUsuario = modificadoPorUsuario;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
