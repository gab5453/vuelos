// Microservicio.Vuelos.Api/Controllers/V1/LogAuditoriaController.cs

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.LogAuditoria;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Api.Controllers.V1;

/// <summary>
/// Controlador para la consulta y registro de logs de auditoría.
/// Permite rastrear cambios en las tablas del sistema.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/log-auditoria")]
[Authorize]
public class LogAuditoriaController : ControllerBase
{
    private readonly ILogAuditoriaService _logAuditoriaService;

    public LogAuditoriaController(ILogAuditoriaService logAuditoriaService)
    {
        _logAuditoriaService = logAuditoriaService;
    }

    // -------------------------------------------------------------------------
    // POST /api/v1/log-auditoria/buscar
    // -------------------------------------------------------------------------
    /// <summary>
    /// Búsqueda paginada de logs de auditoría con filtros.
    /// </summary>
    [HttpPost("buscar")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<LogAuditoriaResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar(
        [FromBody] LogAuditoriaFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var result = await _logAuditoriaService.BuscarAsync(filtro, cancellationToken);
        return Ok(ApiResponse<DataPagedResult<LogAuditoriaResponse>>.Ok(result, "Consulta paginada de auditoría exitosa."));
    }

    // -------------------------------------------------------------------------
    // POST /api/v1/log-auditoria
    // -------------------------------------------------------------------------
    /// <summary>
    /// Registra manualmente un evento de auditoría (opcional, la mayoría son automáticos via triggers).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<LogAuditoriaResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(
        [FromBody] CrearLogAuditoriaRequest request,
        CancellationToken cancellationToken)
    {
        request.CreadoPorUsuario = ObtenerUsernameDelToken();
        request.Ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        request.ServicioOrigen = "Microservicio.Vuelos.Api";
        request.EquipoOrigen = Environment.MachineName;

        var result = await _logAuditoriaService.CrearAsync(request, cancellationToken);

        return Ok(ApiResponse<LogAuditoriaResponse>.Ok(result, "Log de auditoría registrado exitosamente."));
    }

    private string ObtenerUsernameDelToken()
    {
        return User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName)?.Value
            ?? User.Identity?.Name
            ?? "sistema";
    }
}
