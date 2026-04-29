namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PaisRepository : IPaisRepository
{
    private readonly VuelosDbContext _context;

    public PaisRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<PaisEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Paises.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<PaisEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Paises.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(PaisEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Paises.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(PaisEntity entity)
    {
        _context.Paises.Update(entity);
    }

    public void Eliminar(PaisEntity entity)
    {
        _context.Paises.Remove(entity);
    }
}
