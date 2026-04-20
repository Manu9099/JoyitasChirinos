using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class EncargoConfiguration : IEntityTypeConfiguration<Encargo>
{
    public void Configure(EntityTypeBuilder<Encargo> b)
    {
        b.ToTable("encargos");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.Numero).HasColumnName("numero");
        b.Property(x => x.ClienteId).HasColumnName("cliente_id");
        b.Property(x => x.UsuarioId).HasColumnName("usuario_id");
        b.Property(x => x.Descripcion).HasColumnName("descripcion").IsRequired();
        b.Property(x => x.Material).HasColumnName("material").HasConversion<string>().IsRequired();

      b.Property(x => x.PesoEstimado)
    .HasColumnName("peso_estimado_g")
    .HasColumnType("numeric(8,3)");

b.Property(x => x.PrecioAcordado)
    .HasColumnName("precio_acordado")
    .HasColumnType("numeric(10,2)")
    .IsRequired();

        b.Property(x => x.Adelanto)
        .HasColumnName("adelanto")
        .HasColumnType("numeric(10,2)")
        .IsRequired();

        b.Property(x => x.Estado).HasColumnName("estado").HasConversion<string>().IsRequired();
        b.Property(x => x.FechaEntrega)
        .HasColumnName("fecha_entrega")
        .HasColumnType("timestamp without time zone");
        b.Property(x => x.FotoReferenciaUrl).HasColumnName("foto_referencia_url");
        b.Property(x => x.Notas).HasColumnName("notas");
        b.Property(x => x.CreatedAt)
         .HasColumnName("created_at")
        .HasColumnType("timestamp without time zone");

        b.Property(x => x.UpdatedAt)
        .HasColumnName("updated_at")
        .HasColumnType("timestamp without time zone");

        b.HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.ClienteId);
      b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId);
    }
}