using Asp.Versioning;
namespace Microservicio.Vuelos.Api.Controllers.V1;

using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("v{version:apiVersion}/vuelos")]
[ApiVersion("1.0")]
public class VuelosController : ControllerBase
{
    private readonly IVueloService _vueloService;

    public VuelosController(IVueloService vueloService)
    {
        _vueloService = vueloService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<VueloResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<DataPagedResult<VueloResponse>>>> GetVuelos(
        [FromQuery] VueloFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var result = await _vueloService.BuscarVuelosAsync(filtro, cancellationToken);

        var meta = new PaginationMeta
        {
            Page = result.PaginaActual,
            PageSize = result.TamanoPagina,
            Total = result.TotalRegistros,
            TotalPages = result.TotalPaginas
        };

        return Ok(ApiResponse<DataPagedResult<VueloResponse>>.Exitoso(result, "Vuelos encontrados exitosamente", meta));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<VueloResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VueloResponse>>> GetVueloById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _vueloService.GetVueloByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound(ApiResponse<object>.Fallido("Vuelo no encontrado"));

        return Ok(ApiResponse<VueloResponse>.Exitoso(result, "Vuelo encontrado"));
    }

    [HttpGet("{id}/escalas")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EscalaResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<EscalaResponse>>>> GetEscalas(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _vueloService.GetEscalasAsync(id, cancellationToken);
        return Ok(ApiResponse<IEnumerable<EscalaResponse>>.Exitoso(result, "Escalas consultadas exitosamente"));
    }

    [HttpGet("{id}/asientos")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AsientoResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<AsientoResponse>>>> GetAsientos(
        int id,
        [FromQuery] bool? disponible,
        [FromQuery] string? clase,
        CancellationToken cancellationToken)
    {
        var result = await _vueloService.GetAsientosAsync(id, disponible, clase, cancellationToken);
        return Ok(ApiResponse<IEnumerable<AsientoResponse>>.Exitoso(result, "Asientos consultados exitosamente"));
    }
}

