using Microservicio.Vuelos.Business.DTOs.Facturacion;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

/// <summary>
/// Implementación del servicio de negocio para facturación.
/// Aplica validaciones, reglas de negocio y coordina
/// operaciones con la capa de gestión de datos.
/// No accede directamente a repositorios ni a EF Core.
/// </summary>
public class FacturacionService : IFacturacionService
{
    private readonly IFacturacionDataService _facturacionDataService;

    public FacturacionService(
        IFacturacionDataService facturacionDataService)
    {
        _facturacionDataService = facturacionDataService;
    }

    // =========================================================================
    // Comandos (Escritura)
    // =========================================================================

    public async Task<FacturacionResponse> CrearAsync(
        CrearFacturacionRequest request, 
        CancellationToken cancellationToken = default)
    {
        var errors = FacturacionValidator.ValidarCreacion(request);
        if (errors.Any())
            throw new ValidationException("Error en validación de facturación.", errors);

        var dataModel = FacturacionBusinessMapper.ToDataModel(request);

        var resultModel = await _facturacionDataService.CrearAsync(dataModel, cancellationToken);

        return FacturacionBusinessMapper.ToResponse(resultModel);
    }

    public async Task<FacturacionResponse> ActualizarAsync(
        ActualizarFacturacionRequest request, 
        CancellationToken cancellationToken = default)
    {
        var errors = FacturacionValidator.ValidarActualizacion(request);
        if (errors.Any())
            throw new ValidationException("Error en validación de actualización de facturación.", errors);

        var existingModel = await _facturacionDataService.ObtenerPorGuidAsync(request.GuidFactura, cancellationToken);
        if (existingModel == null)
            throw new NotFoundException($"No se encontró la factura con guid {request.GuidFactura}");

        var updatedModel = FacturacionBusinessMapper.ToDataModel(request, existingModel);

        var resultModel = await _facturacionDataService.ActualizarAsync(updatedModel, cancellationToken)
            ?? throw new BusinessException("No se pudo actualizar la facturación.");

        return FacturacionBusinessMapper.ToResponse(resultModel);
    }

    // =========================================================================
    // Consultas (Lectura)
    // =========================================================================

    public async Task<FacturacionResponse> ObtenerPorGuidAsync(
        Guid guidFactura, 
        CancellationToken cancellationToken = default)
    {
        var model = await _facturacionDataService.ObtenerPorGuidAsync(guidFactura, cancellationToken);
        if (model == null)
            throw new NotFoundException($"Facturación con guid {guidFactura} no encontrada.");

        return FacturacionBusinessMapper.ToResponse(model);
    }

    public async Task<FacturacionResponse> ObtenerPorNumeroAsync(
        string numeroFactura, 
        CancellationToken cancellationToken = default)
    {
        var model = await _facturacionDataService.ObtenerPorNumeroAsync(numeroFactura, cancellationToken);
        if (model == null)
            throw new NotFoundException($"Facturación con número {numeroFactura} no encontrada.");

        return FacturacionBusinessMapper.ToResponse(model);
    }

    public async Task<DataPagedResult<FacturacionResponse>> BuscarAsync(
    FacturacionFiltroRequest request,
    CancellationToken cancellationToken = default)
    {
        var filtro = new FacturacionFiltroDataModel
        {
            Estado = request.Estado,
            IdCliente = request.IdCliente,
            FechaEmisionInicio = request.FechaEmisionInicio,
            FechaEmisionFin = request.FechaEmisionFin,
            PaginaActual = request.Page,
            TamanioPagina = request.PageSize
        };

        var pagedResult = await _facturacionDataService.BuscarAsync(filtro, cancellationToken);

        // ✅ constructor con propiedades en español
        var items = pagedResult.Items
            .Select(FacturacionBusinessMapper.ToResponse)
            .ToList();

        return new DataPagedResult<FacturacionResponse>(
            items,
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }

    // =========================================================================
    // Operaciones de Estado
    // =========================================================================

    public async Task EliminarLogicoAsync(
        Guid guidFactura, 
        string usuarioModificador, 
        CancellationToken cancellationToken = default)
    {
        var model = await _facturacionDataService.ObtenerPorGuidAsync(guidFactura, cancellationToken);
        if (model == null)
            throw new NotFoundException($"Facturación con guid {guidFactura} no encontrada.");

        var result = await _facturacionDataService.EliminarLogicoAsync(guidFactura, usuarioModificador, cancellationToken);
        if (!result)
            throw new BusinessException("No se pudo eliminar lógicamente la factura.");
    }
}
