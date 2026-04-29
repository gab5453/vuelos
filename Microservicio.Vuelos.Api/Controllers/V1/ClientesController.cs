using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Cliente;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace Microservicio.Vuelos.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<ClienteResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] ClienteFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var result = await _clienteService.BuscarAsync(filtro, cancellationToken);
        return Ok(ApiResponse<DataPagedResult<ClienteResponse>>.Ok(result, "Consulta paginada exitosa."));
    }

    [HttpGet("{guidCliente:guid}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorGuid(
        Guid guidCliente,
        CancellationToken cancellationToken)
    {
        var result = await _clienteService.ObtenerPorGuidAsync(guidCliente, cancellationToken);
        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpGet("identificacion/{tipoIdentificacion}/{numeroIdentificacion}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorIdentificacion(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken)
    {
        var result = await _clienteService.ObtenerPorNumeroIdentificacionAsync(
            tipoIdentificacion,
            numeroIdentificacion,
            cancellationToken);

        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Consulta exitosa."));
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(
        [FromBody] CrearClienteRequest request,
        CancellationToken cancellationToken)
    {
        request.CreadoPorUsuario = ObtenerUsernameDelToken();
        request.ModificacionIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        request.ServicioOrigen = "Microservicio.Vuelos.Api";

        var result = await _clienteService.CrearAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(ObtenerPorGuid),
            new { guidCliente = result.GuidCliente },
            ApiResponse<ClienteResponse>.Ok(result, "Cliente creado exitosamente."));
    }

    [HttpPut("{guidCliente:guid}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        Guid guidCliente,
        [FromBody] ActualizarClienteRequest request,
        CancellationToken cancellationToken)
    {
        request.GuidCliente = guidCliente;
        request.ModificadoPorUsuario = ObtenerUsernameDelToken();
        request.ModificacionIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        request.ServicioOrigen = "Microservicio.Vuelos.Api";

        var result = await _clienteService.ActualizarAsync(request, cancellationToken);
        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Cliente actualizado exitosamente."));
    }

    [HttpPatch("{guidCliente:guid}/estado/{nuevoEstado}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarEstado(
        Guid guidCliente,
        string nuevoEstado,
        CancellationToken cancellationToken)
    {
        var modificadoPor = ObtenerUsernameDelToken();
        var result = await _clienteService.CambiarEstadoAsync(
            guidCliente,
            nuevoEstado,
            modificadoPor,
            cancellationToken);

        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Estado actualizado exitosamente."));
    }

    [HttpDelete("{guidCliente:guid}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarLogico(
        Guid guidCliente,
        CancellationToken cancellationToken)
    {
        var eliminadoPor = ObtenerUsernameDelToken();
        await _clienteService.EliminarLogicoAsync(
            guidCliente,
            eliminadoPor,
            cancellationToken);

        return Ok(ApiResponse<string>.Ok("OK", "Cliente eliminado logicamente."));
    }

    private string ObtenerUsernameDelToken()
    {
        return User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName)?.Value
            ?? User.Identity?.Name
            ?? "sistema";
    }
}
