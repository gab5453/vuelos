namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AsientoConfiguration : IEntityTypeConfiguration<AsientoEntity>
{
    public void Configure(EntityTypeBuilder<AsientoEntity> builder)
    {
        builder.ToTable("Asiento", "vuelos");
        
        builder.HasKey(x => x.Id_asiento);
        
        builder.Property(x => x.Numero_asiento)
            .IsRequired()
            .HasMaxLength(10);
            
        builder.Property(x => x.Clase)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Precio_extra)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
            
        builder.Property(x => x.Disponible)
            .IsRequired();

        builder.HasOne(x => x.Vuelo)
            .WithMany(v => v.Asientos)
            .HasForeignKey(x => x.Id_vuelo)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
