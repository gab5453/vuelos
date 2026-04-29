namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CiudadConfiguration : IEntityTypeConfiguration<CiudadEntity>
{
    public void Configure(EntityTypeBuilder<CiudadEntity> builder)
    {
        builder.ToTable("Ciudad", "aero");
        
        builder.HasKey(x => x.Id_ciudad);
        
        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(x => x.Pais)
            .WithMany(p => p.Ciudades)
            .HasForeignKey(x => x.Id_pais)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
