namespace Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

public class VuelosDbContext : DbContext
{
    public VuelosDbContext(DbContextOptions<VuelosDbContext> options)
        : base(options) { }

    // Esquema: seg (Seguridad)
    public DbSet<UsuarioAppEntity> UsuariosApp => Set<UsuarioAppEntity>();
    public DbSet<RolEntity> Roles => Set<RolEntity>();
    public DbSet<UsuarioRolEntity> UsuariosRoles => Set<UsuarioRolEntity>();
    public DbSet<LogAuditoriaEntity> LogsAuditoria => Set<LogAuditoriaEntity>();

    // Esquema: crm (Clientes)
    public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();
    public DbSet<PasajeroEntity> Pasajeros => Set<PasajeroEntity>();

    // Esquema: ventas (Facturación y Reservas)
    public DbSet<FacturacionEntity> Facturaciones => Set<FacturacionEntity>();
    public DbSet<ReservaEntity> Reservas => Set<ReservaEntity>();
    public DbSet<BoletoEntity> Boletos => Set<BoletoEntity>();
    public DbSet<EquipajeEntity> Equipajes => Set<EquipajeEntity>();

    // Esquema: aero (Catálogo Aero)
    public DbSet<PaisEntity> Paises => Set<PaisEntity>();
    public DbSet<CiudadEntity> Ciudades => Set<CiudadEntity>();
    public DbSet<AeropuertoEntity> Aeropuertos => Set<AeropuertoEntity>();

    // Esquema: vuelos (Operación)
    public DbSet<VueloEntity> Vuelos => Set<VueloEntity>();
    public DbSet<EscalaEntity> Escalas => Set<EscalaEntity>();
    public DbSet<AsientoEntity> Asientos => Set<AsientoEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(VuelosDbContext).Assembly
        );
    }
}
