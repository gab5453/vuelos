using Asp.Versioning;
namespace Microservicio.Vuelos.Api.Controllers.V1;

using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("v{version:apiVersion}/reservas")]
[ApiVersion("1.0")]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _reservaService;

    public ReservasController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<ReservaResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<DataPagedResult<ReservaResponse>>>> GetReservas(
        [FromQuery] ReservaFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var result = await _reservaService.BuscarReservasAsync(filtro, cancellationToken);

        var meta = new PaginationMeta
        {
            Page = result.PaginaActual,
            PageSize = result.TamanoPagina,
            Total = result.TotalRegistros,
            TotalPages = result.TotalPaginas
        };

        return Ok(ApiResponse<DataPagedResult<ReservaResponse>>.Exitoso(result, "Reservas encontradas exitosamente", meta));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ReservaResponse>>> CrearReserva(
        [FromBody] CrearReservaRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _reservaService.CrearReservaAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(CrearReserva), new { id = response.IdReserva }, ApiResponse<ReservaResponse>.Exitoso(response, "Reserva creada exitosamente"));
    }

    [HttpPatch("{id}/estado")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ReservaResponse>>> ActualizarEstado(
        int id,
        [FromBody] ActualizarEstadoReservaRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _reservaService.ActualizarEstadoAsync(id, request, cancellationToken);
        return Ok(ApiResponse<ReservaResponse>.Exitoso(response, "Estado de reserva actualizado exitosamente"));
    }
}
