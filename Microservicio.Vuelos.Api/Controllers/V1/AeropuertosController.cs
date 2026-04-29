using Asp.Versioning;
using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Api.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/aeropuertos")]
[ApiVersion("1.0")]
public class AeropuertosController : ControllerBase
{
    private readonly IAeropuertoService _aeropuertoService;

    public AeropuertosController(IAeropuertoService aeropuertoService)
    {
        _aeropuertoService = aeropuertoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<AeropuertoResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<DataPagedResult<AeropuertoResponse>>>> GetAeropuertos(
        [FromQuery] AeropuertoFiltroRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _aeropuertoService.GetAeropuertosAsync(request, cancellationToken);
        
        var meta = new PaginationMeta
        {
            Page = result.PaginaActual,
            PageSize = result.TamanoPagina,
            Total = result.TotalRegistros,
            TotalPages = result.TotalPaginas
        };

        return Ok(ApiResponse<DataPagedResult<AeropuertoResponse>>.Exitoso(result, "Aeropuertos consultados exitosamente", meta));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AeropuertoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AeropuertoResponse>>> GetAeropuertoById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _aeropuertoService.GetAeropuertoByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound(ApiResponse<object>.Fallido("Aeropuerto no encontrado"));

        return Ok(ApiResponse<AeropuertoResponse>.Exitoso(result, "Aeropuerto encontrado"));
    }
}
