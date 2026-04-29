namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VueloConfiguration : IEntityTypeConfiguration<VueloEntity>
{
    public void Configure(EntityTypeBuilder<VueloEntity> builder)
    {
        builder.ToTable("Vuelo", "vuelos");
        
        builder.HasKey(x => x.Id_vuelo);
        
        builder.Property(x => x.Numero_vuelo).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Fecha_hora_salida).IsRequired();
        builder.Property(x => x.Fecha_hora_llegada).IsRequired();
        builder.Property(x => x.Duracion_min).IsRequired();
        
        builder.Property(x => x.Precio_base)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Capacidad_total).IsRequired();
            
        builder.Property(x => x.Estado_vuelo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(x => x.AeropuertoOrigen)
            .WithMany()
            .HasForeignKey(x => x.Id_aeropuerto_origen)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AeropuertoDestino)
            .WithMany()
            .HasForeignKey(x => x.Id_aeropuerto_destino)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
