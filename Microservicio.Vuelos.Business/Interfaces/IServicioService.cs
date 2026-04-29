using Microservicio.Vuelos.Business.DTOs;
using Microservicio.Vuelos.Business.DTOs.Servicio;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IServicioService
{
    Task<ServicioResponse?> ObtenerPorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default);

    Task<ServicioResponse?> ObtenerDetallePorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default);

    Task<PagedResultado<ServicioResumenResponse>> ListarOBuscarAsync(
        ServicioFiltroRequest filtro,
        CancellationToken cancellationToken = default);

    Task<PagedResultado<ServicioResponse>> ListarEntidadesPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    Task<ServicioResponse> CrearAsync(CrearServicioRequest request, CancellationToken cancellationToken = default);

    Task<ServicioResponse> ActualizarAsync(ActualizarServicioRequest request, CancellationToken cancellationToken = default);

    Task EliminarAsync(Guid guidServicio, CancellationToken cancellationToken = default);
}
