using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Queries;
using Microservicio.Vuelos.DataAccess.Queries.Interfaces;
using Microservicio.Vuelos.DataAccess.Repositories;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly VuelosDbContext _context;

    // Usuarios
    public IUsuarioRepository UsuarioRepository { get; }
    public IRolRepository RolRepository { get; }

    // Clientes
    public IClienteRepository ClienteRepository { get; }
    // Catálogo Aero (aero)
    public IPaisRepository PaisRepository { get; }
    public ICiudadRepository CiudadRepository { get; }
    public IAeropuertoRepository AeropuertoRepository { get; }

    // Operación (vuelos)
    public IVueloRepository VueloRepository { get; }
    public IEscalaRepository EscalaRepository { get; }
    public IAsientoRepository AsientoRepository { get; }

    // Ventas (ventas)
    public IReservaRepository ReservaRepository { get; }
    public IBoletoRepository BoletoRepository { get; }
    public IEquipajeRepository EquipajeRepository { get; }

    // CRM
    public IPasajeroRepository PasajeroRepository { get; }
    // Facturación
    public IFacturacionRepository FacturacionRepository { get; }

    // Auditoría
    public ILogAuditoriaRepository LogAuditoriaRepository { get; }

    public IServicioRepository ServicioRepository { get; }
    public ITipoServicioRepository TipoServicioRepository { get; }
    public ServicioQueryRepository ServicioQueryRepository { get; }

    // Queries (solo lectura — AsNoTracking)
    public UsuarioQueryRepository UsuarioQueryRepository { get; }
    public ClienteQueryRepository ClienteQueryRepository { get; }

    public IFacturacionQueryRepository FacturacionQueryRepository { get; }
    public ILogAuditoriaQueryRepository LogAuditoriaQueryRepository { get; }

    public UnitOfWork(VuelosDbContext context)
    {
        _context = context;

        UsuarioRepository = new UsuarioRepository(_context);
        RolRepository = new RolRepository(_context);
        ClienteRepository = new ClienteRepository(_context);

        PaisRepository = new PaisRepository(_context);
        CiudadRepository = new CiudadRepository(_context);
        AeropuertoRepository = new AeropuertoRepository(_context);

        VueloRepository = new VueloRepository(_context);
        EscalaRepository = new EscalaRepository(_context);
        AsientoRepository = new AsientoRepository(_context);

        ReservaRepository = new ReservaRepository(_context);
        BoletoRepository = new BoletoRepository(_context);
        EquipajeRepository = new EquipajeRepository(_context);

        PasajeroRepository = new PasajeroRepository(_context);

        FacturacionRepository = new FacturacionRepository(_context);
        LogAuditoriaRepository = new LogAuditoriaRepository(_context);

        ServicioRepository = new ServicioRepository(_context);
        TipoServicioRepository = new TipoServicioRepository(_context);
        ServicioQueryRepository = new ServicioQueryRepository(_context);

        UsuarioQueryRepository = new UsuarioQueryRepository(_context);
        ClienteQueryRepository = new ClienteQueryRepository(_context);

        FacturacionQueryRepository = new FacturacionQueryRepository(_context);
        LogAuditoriaQueryRepository = new LogAuditoriaQueryRepository(_context);
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
