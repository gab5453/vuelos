namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class EscalaRepository : IEscalaRepository
{
    private readonly VuelosDbContext _context;

    public EscalaRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<EscalaEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<EscalaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Escalas.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(EscalaEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Escalas.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(EscalaEntity entity)
    {
        _context.Escalas.Update(entity);
    }

    public void Eliminar(EscalaEntity entity)
    {
        _context.Escalas.Remove(entity);
    }
}
