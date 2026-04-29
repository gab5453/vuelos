using Microservicio.Vuelos.DataAccess.Common;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Vuelos.DataAccess.Repositories;

/// <summary>
/// Implementación de ILogAuditoriaRepository.
/// </summary>
public class LogAuditoriaRepository : ILogAuditoriaRepository
{
    private readonly VuelosDbContext _context;

    public LogAuditoriaRepository(VuelosDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Query base reutilizable
    // -------------------------------------------------------------------------

    /// <summary>
    /// Excluye los logs eliminados de las consultas base.
    /// </summary>
    private IQueryable<LogAuditoriaEntity> QueryVigentes => _context.LogsAuditoria.Where(l => !l.EsEliminadoLog);

    // -------------------------------------------------------------------------
    // Lecturas simples
    // -------------------------------------------------------------------------

    public async Task<LogAuditoriaEntity?> ObtenerPorIdAsync(long idLog, CancellationToken cancellationToken = default)
    {
        return await QueryVigentes.FirstOrDefaultAsync(l => l.IdLog == idLog, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Lecturas paginadas
    // -------------------------------------------------------------------------

    public async Task<PagedResult<LogAuditoriaEntity>> ObtenerTodosPaginadoAsync(int paginaActual, int tamanioPagina, CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderByDescending(l => l.FechaUtc);
        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<LogAuditoriaEntity>.Vacio(paginaActual, tamanioPagina);

        var items = await query.Skip((paginaActual - 1) * tamanioPagina).Take(tamanioPagina).ToListAsync(cancellationToken);
        return new PagedResult<LogAuditoriaEntity>(items, totalRegistros, paginaActual, tamanioPagina);
    }

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    public async Task AgregarAsync(LogAuditoriaEntity log, CancellationToken cancellationToken = default)
    {
        await _context.LogsAuditoria.AddAsync(log, cancellationToken);
    }

    public void EliminarLogico(LogAuditoriaEntity log)
    {
        log.EsEliminadoLog = true;
        _context.LogsAuditoria.Update(log);
    }
}
