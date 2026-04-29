namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PasajeroConfiguration : IEntityTypeConfiguration<PasajeroEntity>
{
    public void Configure(EntityTypeBuilder<PasajeroEntity> builder)
    {
        builder.ToTable("Pasajero", "ventas");
        
        builder.HasKey(x => x.Id_pasajero);
        
        builder.Property(x => x.Nombre)
            .HasColumnName("nombre_pasajero")
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Apellido)
            .HasColumnName("apellido_pasajero")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Documento_identidad)
            .HasColumnName("numero_documento_pasajero")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property<string>("tipo_documento_pasajero")
            .HasColumnName("tipo_documento_pasajero")
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.Documento_identidad).IsUnique();

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.Id_cliente)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
