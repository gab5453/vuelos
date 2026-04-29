using Microservicio.Vuelos.DataAccess.Common;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Vuelos.DataAccess.Queries;

public sealed record ClienteResumenDto(
    int IdCliente,
    Guid GuidCliente,
    string? Nombres,
    string? Apellidos,
    string? RazonSocial,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string Correo,
    string? Telefono,
    string? Direccion,
    DateTime? FechaNacimiento,
    string? Nacionalidad,
    string? Genero,
    string Estado
);

public sealed record ClienteDetalleDto(
    int IdCliente,
    Guid GuidCliente,
    int IdUsuario,
    string? Nombres,
    string? Apellidos,
    string? RazonSocial,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string Correo,
    string? Telefono,
    string? Direccion,
    int IdCiudadResidencia,
    int IdPaisNacionalidad,
    DateTime? FechaNacimiento,
    string? Nacionalidad,
    string? Genero,
    string Estado,
    bool EsEliminado,
    string? CreadoPorUsuario,
    DateTimeOffset FechaRegistroUtc,
    string? ModificadoPorUsuario,
    DateTimeOffset? FechaModificacionUtc
);

public class ClienteQueryRepository
{
    private readonly VuelosDbContext _context;

    public ClienteQueryRepository(VuelosDbContext context)
    {
        _context = context;
    }

    private IQueryable<ClienteEntity> QueryVigentes =>
        _context.Clientes
                .AsNoTracking()
                .Where(c => !c.EsEliminado);

    public async Task<PagedResult<ClienteResumenDto>> ListarClientesAsync(
        int paginaActual,
        int tamanioPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(c => c.IdCliente);
        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ClienteResumenDto>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .Select(c => new ClienteResumenDto(
                c.IdCliente,
                c.GuidCliente,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Direccion,
                c.FechaNacimiento,
                c.Nacionalidad,
                c.Genero,
                c.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteResumenDto>(items, totalRegistros, paginaActual, tamanioPagina);
    }

    public async Task<ClienteDetalleDto?> ObtenerDetalleAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Where(c => c.GuidCliente == guidCliente)
            .Select(c => new ClienteDetalleDto(
                c.IdCliente,
                c.GuidCliente,
                c.IdUsuario,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Direccion,
                c.IdCiudadResidencia,
                c.IdPaisNacionalidad,
                c.FechaNacimiento,
                c.Nacionalidad,
                c.Genero,
                c.Estado,
                c.EsEliminado,
                c.CreadoPorUsuario,
                c.FechaRegistroUtc,
                c.ModificadoPorUsuario,
                c.FechaModificacionUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<ClienteResumenDto>> BuscarClientesAsync(
        string termino,
        int paginaActual,
        int tamanioPagina,
        CancellationToken cancellationToken = default)
    {
        var terminoLower = termino.Trim().ToLower();
        var query = QueryVigentes
            .Where(c =>
                (c.Nombres != null && c.Nombres.ToLower().Contains(terminoLower)) ||
                (c.Apellidos != null && c.Apellidos.ToLower().Contains(terminoLower)) ||
                (c.RazonSocial != null && c.RazonSocial.ToLower().Contains(terminoLower)) ||
                c.Correo.ToLower().Contains(terminoLower) ||
                c.NumeroIdentificacion.ToLower().Contains(terminoLower))
            .OrderBy(c => c.IdCliente);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ClienteResumenDto>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .Select(c => new ClienteResumenDto(
                c.IdCliente,
                c.GuidCliente,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Direccion,
                c.FechaNacimiento,
                c.Nacionalidad,
                c.Genero,
                c.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteResumenDto>(items, totalRegistros, paginaActual, tamanioPagina);
    }

    public async Task<PagedResult<ClienteResumenDto>> ListarClientesPorEstadoAsync(
        string estado,
        int paginaActual,
        int tamanioPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes
            .Where(c => c.Estado == estado)
            .OrderBy(c => c.IdCliente);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ClienteResumenDto>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .Select(c => new ClienteResumenDto(
                c.IdCliente,
                c.GuidCliente,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Direccion,
                c.FechaNacimiento,
                c.Nacionalidad,
                c.Genero,
                c.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteResumenDto>(items, totalRegistros, paginaActual, tamanioPagina);
    }
}
