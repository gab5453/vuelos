using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Services;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Services;

namespace Microservicio.Vuelos.Api.Extensions;

/// <summary>
/// Registra todos los servicios del módulo de Usuarios (capas 1, 2 y 3).
/// Se llama desde Program.cs con builder.Services.AddUsuariosModule(configuration).
/// Cada módulo del equipo tiene su propia extension de este tipo.
/// </summary>
public static class UsuariosServiceExtensions
{
    public static IServiceCollection AddUsuariosModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Capa 1 — DbContext (SQL Server)
        services.AddDbContext<VuelosDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        // Capa 2 — Unit of Work y servicios de datos
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsuarioDataService, UsuarioDataService>();
        services.AddScoped<IRolDataService, RolDataService>();

        // Capa 3 — Servicios de negocio
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
