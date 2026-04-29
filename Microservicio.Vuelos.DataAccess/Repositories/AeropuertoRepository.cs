namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Common;

public class AeropuertoRepository : IAeropuertoRepository
{
    private readonly VuelosDbContext _context;

    public AeropuertoRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<AeropuertoEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Aeropuertos.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<AeropuertoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Aeropuertos
            .Include(a => a.Ciudad)
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(AeropuertoEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Aeropuertos.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(AeropuertoEntity entity)
    {
        _context.Aeropuertos.Update(entity);
    }

    public void Eliminar(AeropuertoEntity entity)
    {
        _context.Aeropuertos.Remove(entity);
    }

    public async Task<PagedResult<AeropuertoEntity>> BuscarAeropuertosAsync(
        string? estado,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Aeropuertos.AsQueryable();

        if (!string.IsNullOrEmpty(estado))
        {
            query = query.Where(a => a.Estado == estado);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Include(a => a.Ciudad)
            .OrderBy(a => a.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<AeropuertoEntity>(items, total, page, pageSize);
    }
}
