using Asp.Versioning;
using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Pais;
using Microservicio.Vuelos.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Api.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/paises")]
[ApiVersion("1.0")]
public class PaisesController : ControllerBase
{
    private readonly IPaisService _paisService;

    public PaisesController(IPaisService paisService)
    {
        _paisService = paisService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PaisResponse>>>> GetPaises(CancellationToken cancellationToken)
    {
        var result = await _paisService.GetPaisesAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<PaisResponse>>.Exitoso(result, "Países consultados exitosamente"));
    }
}