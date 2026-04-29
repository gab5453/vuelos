using Microservicio.Vuelos.DataAccess.Queries;
using Microservicio.Vuelos.DataAccess.Queries.Interfaces;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
namespace Microservicio.Vuelos.DataManagement.Interfaces;

/// <summary>
/// Unidad de trabajo: centraliza repositorios y el guardado transaccional.
/// La capa de negocio nunca llama SaveChanges directamente;
/// siempre lo hace a través de este contrato.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Usuarios
    IUsuarioRepository UsuarioRepository { get; }
    IRolRepository RolRepository { get; }
    UsuarioQueryRepository UsuarioQueryRepository { get; }

    // Clientes
    IClienteRepository ClienteRepository { get; }
    ClienteQueryRepository ClienteQueryRepository { get; }

    // Catálogo Aero (aero)
    IPaisRepository PaisRepository { get; }
    ICiudadRepository CiudadRepository { get; }
    IAeropuertoRepository AeropuertoRepository { get; }

    // Operación (vuelos)
    IVueloRepository VueloRepository { get; }
    IEscalaRepository EscalaRepository { get; }
    IAsientoRepository AsientoRepository { get; }

    // Ventas (ventas)
    IReservaRepository ReservaRepository { get; }
    IBoletoRepository BoletoRepository { get; }
    IEquipajeRepository EquipajeRepository { get; }

    // CRM
    IPasajeroRepository PasajeroRepository { get; }
    // Facturación
    IFacturacionRepository FacturacionRepository { get; }
    IFacturacionQueryRepository FacturacionQueryRepository { get; }

    // Auditoría
    ILogAuditoriaRepository LogAuditoriaRepository { get; }
    ILogAuditoriaQueryRepository LogAuditoriaQueryRepository { get; }

    // Catálogos adicionales
    IServicioRepository ServicioRepository { get; }
    ITipoServicioRepository TipoServicioRepository { get; }
    ServicioQueryRepository ServicioQueryRepository { get; }

    // Persistencia
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
