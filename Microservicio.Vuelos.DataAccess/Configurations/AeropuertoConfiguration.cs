namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AeropuertoConfiguration : IEntityTypeConfiguration<AeropuertoEntity>
{
    public void Configure(EntityTypeBuilder<AeropuertoEntity> builder)
    {
        builder.ToTable("Aeropuerto", "aero");
        
        builder.HasKey(x => x.Id_aeropuerto);
        
        builder.Property(x => x.Codigo_iata)
            .IsRequired()
            .HasMaxLength(3);
            
        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Latitud)
            .HasColumnType("decimal(9,6)");

        builder.Property(x => x.Longitud)
            .HasColumnType("decimal(9,6)");

        builder.Property(x => x.Zona_horaria)
            .HasMaxLength(50);

        builder.HasIndex(x => x.Codigo_iata).IsUnique();

        builder.HasOne(x => x.Ciudad)
            .WithMany(c => c.Aeropuertos)
            .HasForeignKey(x => x.Id_ciudad)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
