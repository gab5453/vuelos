namespace Microservicio.Vuelos.DataAccess.Repositories;

using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class BoletoRepository : IBoletoRepository
{
    private readonly VuelosDbContext _context;

    public BoletoRepository(VuelosDbContext context)
    {
        _context = context;
    }

    public async Task<BoletoEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<BoletoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Boletos.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(BoletoEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Boletos.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(BoletoEntity entity)
    {
        _context.Boletos.Update(entity);
    }

    public void Eliminar(BoletoEntity entity)
    {
        _context.Boletos.Remove(entity);
    }
}
