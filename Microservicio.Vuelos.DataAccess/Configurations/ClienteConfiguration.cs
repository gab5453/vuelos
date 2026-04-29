using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Microservicio.Vuelos.DataAccess.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
{
    public void Configure(EntityTypeBuilder<ClienteEntity> builder)
    {
        // Tabla y esquema
        builder.ToTable("CLIENTES", "crm");

        // [1] Clave primaria e identificación
        builder.HasKey(e => e.IdCliente);

        builder.Property(e => e.IdCliente)
            .HasColumnName("id_cliente")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.GuidCliente)
            .HasColumnName("cliente_guid")
            .HasColumnType("uniqueidentifier")
            .HasDefaultValueSql("NEWID()")
            .IsRequired();

        // [2] Datos funcionales
        builder.Property(e => e.IdUsuario)
            .HasColumnName("id_usuario")
            .IsRequired();

        builder.Property(e => e.Nombres)
            .HasColumnName("nombres")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.Apellidos)
            .HasColumnName("apellidos")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.RazonSocial)
            .HasColumnName("razon_social")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.TipoIdentificacion)
            .HasColumnName("tipo_identificacion")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.NumeroIdentificacion)
            .HasColumnName("numero_identificacion")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Correo)
            .HasColumnName("correo")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(e => e.Direccion)
            .HasColumnName("direccion")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.IdCiudadResidencia)
            .HasColumnName("id_ciudad_residencia")
            .IsRequired();

        builder.Property(e => e.IdPaisNacionalidad)
            .HasColumnName("id_pais_nacionalidad")
            .IsRequired();

        builder.Property(e => e.FechaNacimiento)
            .HasColumnName("fecha_nacimiento")
            .HasColumnType("date")
            .IsRequired(false);

        builder.Property(e => e.Nacionalidad)
            .HasColumnName("nacionalidad")
            .HasMaxLength(80)
            .IsRequired(false);

        builder.Property(e => e.Genero)
            .HasColumnName("genero")
            .HasMaxLength(20)
            .IsRequired(false);

        // [3] Estado y ciclo de vida
        builder.Property(e => e.Estado)
            .HasColumnName("estado")
            .HasColumnType("char(3)")
            .HasDefaultValue("ACT")
            .IsRequired();

        builder.Property(e => e.EsEliminado)
            .HasColumnName("es_eliminado")
            .HasDefaultValue(false)
            .IsRequired();

        // [4] Auditoría
        builder.Property(e => e.CreadoPorUsuario)
            .HasColumnName("creado_por_usuario")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(e => e.FechaRegistroUtc)
            .HasColumnName("fecha_registro_utc")
            .HasColumnType("datetimeoffset")
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        builder.Property(e => e.ModificadoPorUsuario)
            .HasColumnName("modificado_por_usuario")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(e => e.FechaModificacionUtc)
            .HasColumnName("fecha_modificacion_utc")
            .HasColumnType("datetimeoffset")
            .IsRequired(false);

        builder.Property(e => e.ModificacionIp)
            .HasColumnName("modificacion_ip")
            .HasMaxLength(45)
            .IsRequired(false);

        builder.Property(e => e.ServicioOrigen)
            .HasColumnName("servicio_origen")
            .HasMaxLength(100)
            .IsRequired(false);

        // Restricciones UNIQUE
        builder.HasIndex(e => e.GuidCliente)
            .IsUnique()
            .HasDatabaseName("uq_cliente_guid");

        // builder.HasIndex(e => e.IdUsuario)
        //     .IsUnique()
        //     .HasDatabaseName("uq_cliente_usuario");

        builder.HasIndex(e => new { e.TipoIdentificacion, e.NumeroIdentificacion })
            .IsUnique()
            .HasDatabaseName("uq_cliente_numero_id");

        // Índices de consulta frecuente
        builder.HasIndex(e => e.Estado)
            .HasDatabaseName("idx_cliente_estado");

        builder.HasIndex(e => e.Correo)
            .HasDatabaseName("idx_cliente_correo");

        // Relaciones (En el script, la relación está en USUARIO_APP)
        // builder.HasOne(e => e.UsuarioApp)
        //     .WithOne()
        //     .HasForeignKey<ClienteEntity>(e => e.IdUsuario)
        //     .HasConstraintName("fk_cliente_usuario_app")
        //     .OnDelete(DeleteBehavior.NoAction);

        // Filtro global - soft delete
        builder.HasQueryFilter(e => !e.EsEliminado);
    }
}
