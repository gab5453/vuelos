namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PasajeroRepository : IPasajeroRepository
{
    private readonly VuelosDbContext _context;

    public PasajeroRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<PasajeroEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<PasajeroEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(PasajeroEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Pasajeros.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(PasajeroEntity entity)
    {
        _context.Pasajeros.Update(entity);
    }

    public void Eliminar(PasajeroEntity entity)
    {
        _context.Pasajeros.Remove(entity);
    }
}
