using Asp.Versioning;
using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Pasajero;
using Microservicio.Vuelos.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Api.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/pasajeros")]
[ApiVersion("1.0")]
public class PasajerosController : ControllerBase
{
    private readonly IPasajeroService _pasajeroService;

    public PasajerosController(IPasajeroService pasajeroService)
    {
        _pasajeroService = pasajeroService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PasajeroResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<PasajeroResponse>>> RegistrarPasajero(
        [FromBody] PasajeroRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _pasajeroService.RegistrarPasajeroAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetPasajeroById), new { id = result.IdPasajero }, ApiResponse<PasajeroResponse>.Exitoso(result, "Pasajero registrado exitosamente"));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PasajeroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PasajeroResponse>>> GetPasajeroById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _pasajeroService.GetPasajeroByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound(ApiResponse<object>.Fallido("Pasajero no encontrado"));

        return Ok(ApiResponse<PasajeroResponse>.Exitoso(result, "Pasajero encontrado"));
    }
}
