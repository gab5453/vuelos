namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Common;

public class VueloRepository : IVueloRepository
{
    private readonly VuelosDbContext _context;

    public VueloRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<VueloEntity?> ObtenerPorIdAsync(int idVuelo, CancellationToken cancellationToken = default)
    {
        return await _context.Vuelos
            .Include(v => v.AeropuertoOrigen)
            .Include(v => v.AeropuertoDestino)
            .FirstOrDefaultAsync(v => v.Id_vuelo == idVuelo, cancellationToken);
    }

    public async Task<IEnumerable<VueloEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vuelos
            .Include(v => v.AeropuertoOrigen)
            .Include(v => v.AeropuertoDestino)
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(VueloEntity vuelo, CancellationToken cancellationToken = default)
    {
        await _context.Vuelos.AddAsync(vuelo, cancellationToken);
    }

    public void Actualizar(VueloEntity vuelo)
    {
        _context.Vuelos.Update(vuelo);
    }

    public void Eliminar(VueloEntity vuelo)
    {
        _context.Vuelos.Remove(vuelo);
    }

    public async Task<PagedResult<VueloEntity>> BuscarVuelosAsync(
        int idAeropuertoOrigen,
        int idAeropuertoDestino,
        DateTime fechaSalida,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Vuelos
            .Include(v => v.AeropuertoOrigen)
                .ThenInclude(a => a.Ciudad)
            .Include(v => v.AeropuertoDestino)
                .ThenInclude(a => a.Ciudad)
            .Where(v => v.Estado_vuelo == "PROGRAMADO")
            .AsQueryable();

        if (idAeropuertoOrigen > 0)
        {
            query = query.Where(v => v.Id_aeropuerto_origen == idAeropuertoOrigen);
        }

        if (idAeropuertoDestino > 0)
        {
            query = query.Where(v => v.Id_aeropuerto_destino == idAeropuertoDestino);
        }

        query = query.Where(v => v.Fecha_hora_salida.Date == fechaSalida.Date);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<VueloEntity>(items, total, page, pageSize);
    }
}

