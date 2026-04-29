namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AsientoRepository : IAsientoRepository
{
    private readonly VuelosDbContext _context;

    public AsientoRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<AsientoEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Asientos.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<AsientoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Asientos.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(AsientoEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Asientos.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(AsientoEntity entity)
    {
        _context.Asientos.Update(entity);
    }

    public void Eliminar(AsientoEntity entity)
    {
        _context.Asientos.Remove(entity);
    }
}
