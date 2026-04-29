namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EquipajeConfiguration : IEntityTypeConfiguration<EquipajeEntity>
{
    public void Configure(EntityTypeBuilder<EquipajeEntity> builder)
    {
        builder.ToTable("Equipaje", "ventas");
        
        builder.HasKey(x => x.Id_equipaje);
        
        builder.Property(x => x.Tipo_equipaje)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Peso_kg)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.HasOne(x => x.Boleto)
            .WithMany(b => b.Equipajes)
            .HasForeignKey(x => x.Id_boleto)
            .OnDelete(DeleteBehavior.Cascade);

        // CHECK CONSTRAINT: si el tipo es 'MANO', el peso máximo es 10kg
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Equipaje_PesoMano", 
            "Tipo_equipaje <> 'MANO' OR Peso_kg <= 10"));
    }
}
