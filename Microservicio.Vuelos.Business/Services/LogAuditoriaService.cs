using Microservicio.Vuelos.Business.DTOs.LogAuditoria;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

/// <summary>
/// Implementación del servicio de negocio para logs de auditoría.
/// Aplica validaciones básicas y coordina operaciones
/// con la capa de gestión de datos para trazabilidad.
/// No accede directamente a repositorios ni a EF Core.
/// </summary>
public class LogAuditoriaService : ILogAuditoriaService
{
    private readonly ILogAuditoriaDataService _logAuditoriaDataService;

    public LogAuditoriaService(
        ILogAuditoriaDataService logAuditoriaDataService)
    {
        _logAuditoriaDataService = logAuditoriaDataService;
    }

    // =========================================================================
    // Comandos (Escritura)
    // =========================================================================

    public async Task<LogAuditoriaResponse> CrearAsync(
        CrearLogAuditoriaRequest request, 
        CancellationToken cancellationToken = default)
    {
        var errors = LogAuditoriaValidator.ValidarCreacion(request);
        if (errors.Any())
            throw new ValidationException("Error en validación de log de auditoría.", errors);

        var dataModel = LogAuditoriaBusinessMapper.ToDataModel(request);

        var resultModel = await _logAuditoriaDataService.CrearAsync(dataModel, cancellationToken);

        return LogAuditoriaBusinessMapper.ToResponse(resultModel);
    }

    // =========================================================================
    // Consultas (Lectura)
    // =========================================================================

    public async Task<DataPagedResult<LogAuditoriaResponse>> BuscarAsync(
    LogAuditoriaFiltroRequest request,
    CancellationToken cancellationToken = default)
    {
        var filtro = new LogAuditoriaFiltroDataModel
        {
            TablaAfectada = request.TablaAfectada,
            PaginaActual = request.PageNumber,
            TamanioPagina = request.PageSize
        };

        var pagedResult = await _logAuditoriaDataService.BuscarAsync(filtro, cancellationToken);

        // ✅ constructor con propiedades en español
        var items = pagedResult.Items
            .Select(LogAuditoriaBusinessMapper.ToResponse)
            .ToList();

        return new DataPagedResult<LogAuditoriaResponse>(
            items,
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }
}
