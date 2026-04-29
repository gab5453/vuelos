using Microservicio.Vuelos.Business.DTOs.Rol;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services;

public class RolService : IRolService
{
    private readonly IRolDataService _rolDataService;
    private readonly IUsuarioDataService _usuarioDataService;

    public RolService(
        IRolDataService rolDataService,
        IUsuarioDataService usuarioDataService)
    {
        _rolDataService = rolDataService;
        _usuarioDataService = usuarioDataService;
    }

    public async Task<IReadOnlyList<RolResponse>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var roles = await _rolDataService.ObtenerTodosLosRolesAsync(cancellationToken);
        return roles.Select(RolBusinessMapper.ToResponse).ToList();
    }

    public async Task<RolResponse?> ObtenerPorGuidAsync(
        Guid rolGuid,
        CancellationToken cancellationToken = default)
    {
        var model = await _rolDataService.ObtenerRolPorGuidAsync(rolGuid, cancellationToken);

        if (model is null)
            throw new NotFoundException($"No se encontró el rol con GUID '{rolGuid}'.");

        return RolBusinessMapper.ToResponse(model);
    }

    public async Task<RolResponse> CrearAsync(
        CrearRolRequest request,
        CancellationToken cancellationToken = default)
    {
        RolValidator.ValidarCrear(request);

        if (await _rolDataService.ExisteNombreRolAsync(request.NombreRol, cancellationToken))
            throw new ValidationException($"El rol '{request.NombreRol}' ya existe en el catálogo.");

        var dataModel = RolBusinessMapper.ToDataModel(request);
        var creado = await _rolDataService.CrearRolAsync(dataModel, cancellationToken);

        return RolBusinessMapper.ToResponse(creado);
    }

    public async Task EliminarLogicoAsync(
        Guid rolGuid,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var rol = await _rolDataService.ObtenerRolPorGuidAsync(rolGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el rol con GUID '{rolGuid}'.");

        var eliminado = await _rolDataService
            .EliminarLogicoRolAsync(rol.IdRol, modificadoPorUsuario, cancellationToken);

        if (!eliminado)
            throw new NotFoundException($"No se pudo eliminar el rol con GUID '{rolGuid}'.");
    }

    public async Task<IReadOnlyList<RolResponse>> ObtenerRolesDeUsuarioAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el usuario con GUID '{usuarioGuid}'.");

        var roles = await _rolDataService
            .ObtenerRolesDeUsuarioAsync(usuario.IdUsuario, cancellationToken);

        return roles.Select(RolBusinessMapper.ToResponse).ToList();
    }

    public async Task<IReadOnlyList<UsuarioRolResponse>> ObtenerAsignacionesDeUsuarioAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el usuario con GUID '{usuarioGuid}'.");

        var asignaciones = await _rolDataService
            .ObtenerAsignacionesDeUsuarioAsync(usuario.IdUsuario, cancellationToken);

        return asignaciones
            .Select(RolBusinessMapper.ToUsuarioRolResponse)
            .ToList();
    }

    public async Task AsignarRolAsync(
        AsignarRolRequest request,
        CancellationToken cancellationToken = default)
    {
        RolValidator.ValidarAsignacion(request);

        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(request.UsuarioGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el usuario con GUID '{request.UsuarioGuid}'.");

        var rol = await _rolDataService
            .ObtenerRolPorGuidAsync(request.RolGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el rol con GUID '{request.RolGuid}'.");

        if (await _rolDataService.UsuarioTieneRolAsync(usuario.IdUsuario, rol.IdRol, cancellationToken))
            throw new ValidationException(
                $"El usuario ya tiene asignado el rol '{rol.NombreRol}'.");

        await _rolDataService.AsignarRolAsync(
            usuario.IdUsuario,
            rol.IdRol,
            request.EjecutadoPorUsuario,
            cancellationToken);
    }

    public async Task RevocarRolAsync(
        AsignarRolRequest request,
        CancellationToken cancellationToken = default)
    {
        RolValidator.ValidarAsignacion(request);

        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(request.UsuarioGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el usuario con GUID '{request.UsuarioGuid}'.");

        var rol = await _rolDataService
            .ObtenerRolPorGuidAsync(request.RolGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el rol con GUID '{request.RolGuid}'.");

        if (!await _rolDataService.UsuarioTieneRolAsync(usuario.IdUsuario, rol.IdRol, cancellationToken))
            throw new ValidationException(
                $"El usuario no tiene asignado el rol '{rol.NombreRol}'.");

        var revocado = await _rolDataService.RevocarRolAsync(
            usuario.IdUsuario,
            rol.IdRol,
            request.EjecutadoPorUsuario,
            cancellationToken);

        if (!revocado)
            throw new NotFoundException(
                $"No se encontró la asignación de rol '{request.RolGuid}' para el usuario '{request.UsuarioGuid}'.");
    }
}
