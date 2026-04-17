using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class VentaItemConfiguration : IEntityTypeConfiguration<VentaItem>
{
    public void Configure(EntityTypeBuilder<VentaItem> b)
    {
        b.ToTable("venta_items");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.VentaId).HasColumnName("venta_id");
        b.Property(x => x.ProductoId).HasColumnName("producto_id");
        b.Property(x => x.Cantidad).HasColumnName("cantidad");

        b.Property(x => x.PrecioUnitario)
            .HasColumnName("precio_unitario")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        b.Property(x => x.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone");
    }
}