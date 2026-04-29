namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EscalaConfiguration : IEntityTypeConfiguration<EscalaEntity>
{
    public void Configure(EntityTypeBuilder<EscalaEntity> builder)
    {
        builder.ToTable("Escala", "vuelos");
        
        builder.HasKey(x => x.Id_escala);
        
        builder.Property(x => x.Orden).IsRequired();

        builder.HasOne(x => x.Vuelo)
            .WithMany(v => v.Escalas)
            .HasForeignKey(x => x.Id_vuelo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AeropuertoEscala)
            .WithMany()
            .HasForeignKey(x => x.Id_aeropuerto_escala)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Id_aeropuerto_escala).HasColumnName("id_aeropuerto");
    }
}
