// Microservicio.Vuelos.Api/Extensions/FacturacionServiceExtensions.cs

using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Services;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Services;

namespace Microservicio.Vuelos.Api.Extensions;

/// <summary>
/// Registra todos los servicios del módulo de Facturación y Log de Auditoría (capas 2 y 3).
/// Se llama desde Program.cs con builder.Services.AddFacturacionModule().
/// </summary>
public static class FacturacionServiceExtensions
{
    public static IServiceCollection AddFacturacionModule(
        this IServiceCollection services)
    {
        // --- Facturación ---
        // Capa 2 — Servicio de datos
        services.AddScoped<IFacturacionDataService, FacturacionDataService>();
        // Capa 3 — Servicio de negocio
        services.AddScoped<IFacturacionService, FacturacionService>();

        // --- Log Auditoría ---
        // Capa 2 — Servicio de datos
        services.AddScoped<ILogAuditoriaDataService, LogAuditoriaDataService>();
        // Capa 3 — Servicio de negocio
        services.AddScoped<ILogAuditoriaService, LogAuditoriaService>();

        return services;
    }
}
