using Asp.Versioning;
using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Boleto;
using Microservicio.Vuelos.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Api.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/boletos")]
[ApiVersion("1.0")]
public class BoletosController : ControllerBase
{
    private readonly IBoletoService _boletoService;

    public BoletosController(IBoletoService boletoService)
    {
        _boletoService = boletoService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BoletoResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<BoletoResponse>>> EmitirBoleto(
        [FromBody] EmitirBoletoRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _boletoService.EmitirBoletoAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetBoletoById), new { id = result.IdBoleto }, ApiResponse<BoletoResponse>.Exitoso(result, "Boleto emitido exitosamente"));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BoletoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BoletoResponse>>> GetBoletoById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _boletoService.GetBoletoByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound(ApiResponse<object>.Fallido("Boleto no encontrado"));

        return Ok(ApiResponse<BoletoResponse>.Exitoso(result, "Boleto encontrado"));
    }
}
