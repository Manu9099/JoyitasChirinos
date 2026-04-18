using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> b)
    {
        b.ToTable("ventas");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.ClienteId).HasColumnName("cliente_id");
        b.Property(x => x.UsuarioId).HasColumnName("usuario_id");

        b.Property(x => x.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        b.Property(x => x.Descuento)
            .HasColumnName("descuento")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        b.Property(x => x.Total)
            .HasColumnName("total")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        b.Property(x => x.MetodoPago)
            .HasColumnName("metodo_pago")
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Estado)
            .HasColumnName("estado")
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.Notas).HasColumnName("notas");
        b.Property(x => x.Numero).HasColumnName("numero");
        b.Property(x => x.Anulada).HasColumnName("anulada");

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone");

        b.Property(x => x.Fecha)
            .HasColumnName("fecha")
            .HasColumnType("timestamp without time zone");

        b.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.VentaId);
    }
}