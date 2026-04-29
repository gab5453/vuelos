namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Common;

public class ReservaRepository : IReservaRepository
{
    private readonly VuelosDbContext _context;

    public ReservaRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<ReservaEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<ReservaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Reservas.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(ReservaEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Reservas.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(ReservaEntity entity)
    {
        _context.Reservas.Update(entity);
    }

    public void Eliminar(ReservaEntity entity)
    {
        _context.Reservas.Remove(entity);
    }

    public async Task<PagedResult<ReservaEntity>> BuscarReservasAsync(
        int? idCliente,
        string? estado,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Vuelo)
            .AsQueryable();

        if (idCliente.HasValue)
        {
            query = query.Where(r => r.Id_cliente == idCliente.Value);
        }

        if (!string.IsNullOrEmpty(estado))
        {
            query = query.Where(r => r.Estado_reserva == estado);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.Fecha_reserva_utc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ReservaEntity>(items, total, page, pageSize);
    }
}
