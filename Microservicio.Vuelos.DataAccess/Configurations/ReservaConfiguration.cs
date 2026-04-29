namespace Microservicio.Vuelos.DataAccess.Configurations;

using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ReservaConfiguration : IEntityTypeConfiguration<ReservaEntity>
{
    public void Configure(EntityTypeBuilder<ReservaEntity> builder)
    {
        builder.ToTable("RESERVAS", "ventas");
        
        builder.HasKey(x => x.Id_reserva);
        
        builder.Property(x => x.Codigo_reserva)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(x => x.Estado_reserva)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Fecha_reserva_utc)
            .IsRequired();

        builder.HasIndex(x => x.Codigo_reserva).IsUnique();

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.Id_cliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Vuelo)
            .WithMany()
            .HasForeignKey(x => x.Id_vuelo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
