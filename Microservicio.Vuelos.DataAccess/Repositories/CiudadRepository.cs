namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CiudadRepository : ICiudadRepository
{
    private readonly VuelosDbContext _context;

    public CiudadRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<CiudadEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<CiudadEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(CiudadEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Ciudades.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(CiudadEntity entity)
    {
        _context.Ciudades.Update(entity);
    }

    public void Eliminar(CiudadEntity entity)
    {
        _context.Ciudades.Remove(entity);
    }
}
