namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PaisConfiguration : IEntityTypeConfiguration<PaisEntity>
{
    public void Configure(EntityTypeBuilder<PaisEntity> builder)
    {
        builder.ToTable("Pais", "aero");
        
        builder.HasKey(x => x.Id_pais);
        
        builder.Property(x => x.Codigo_iso2)
            .IsRequired()
            .HasMaxLength(2);
            
        builder.Property(x => x.Codigo_iso3)
            .IsRequired()
            .HasMaxLength(3);
            
        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Continente)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Codigo_iso2).IsUnique();
        builder.HasIndex(x => x.Codigo_iso3).IsUnique();
    }
}
