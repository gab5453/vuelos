using Asp.Versioning;
using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Equipaje;
using Microservicio.Vuelos.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Api.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/boletos/{idBoleto}/equipaje")]
[ApiVersion("1.0")]
public class EquipajeController : ControllerBase
{
    private readonly IEquipajeService _equipajeService;

    public EquipajeController(IEquipajeService equipajeService)
    {
        _equipajeService = equipajeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EquipajeResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<EquipajeResponse>>> RegistrarEquipaje(
        int idBoleto,
        [FromBody] RegistrarEquipajeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _equipajeService.RegistrarEquipajeAsync(idBoleto, request, cancellationToken);
        return Ok(ApiResponse<EquipajeResponse>.Exitoso(result, "Equipaje registrado exitosamente"));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EquipajeResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<EquipajeResponse>>>> GetEquipajeByBoleto(
        int idBoleto,
        CancellationToken cancellationToken)
    {
        var result = await _equipajeService.GetEquipajeByBoletoAsync(idBoleto, cancellationToken);
        return Ok(ApiResponse<IEnumerable<EquipajeResponse>>.Exitoso(result, "Equipajes consultados exitosamente"));
    }
}
