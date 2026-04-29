namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class EquipajeRepository : IEquipajeRepository
{
    private readonly VuelosDbContext _context;

    public EquipajeRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<EquipajeEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<EquipajeEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(EquipajeEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Equipajes.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(EquipajeEntity entity)
    {
        _context.Equipajes.Update(entity);
    }

    public void Eliminar(EquipajeEntity entity)
    {
        _context.Equipajes.Remove(entity);
    }
}
