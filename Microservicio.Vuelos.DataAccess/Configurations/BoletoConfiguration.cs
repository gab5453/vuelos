namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BoletoConfiguration : IEntityTypeConfiguration<BoletoEntity>
{
    public void Configure(EntityTypeBuilder<BoletoEntity> builder)
    {
        builder.ToTable("Boleto", "ventas");
        
        builder.HasKey(x => x.Id_boleto);
        
        builder.Property(x => x.Codigo_boleto)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Id_vuelo).IsRequired();
        builder.Property(x => x.Id_asiento).IsRequired();
        builder.Property(x => x.Id_factura).IsRequired();
        builder.Property(x => x.Clase).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Precio_vuelo_base).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Precio_asiento_extra).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Impuestos_boleto).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Cargo_equipaje).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Precio_final).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Estado_boleto).IsRequired().HasMaxLength(20);
            
        builder.Property(x => x.Fecha_emision)
            .IsRequired();

        builder.HasIndex(x => x.Codigo_boleto).IsUnique();

        builder.HasOne(x => x.Reserva)
            .WithMany(r => r.Boletos)
            .HasForeignKey(x => x.Id_reserva)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
