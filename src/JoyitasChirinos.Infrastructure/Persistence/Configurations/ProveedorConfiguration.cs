using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> b)
    {
        b.ToTable("proveedores");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(120).IsRequired();
        b.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(20);
        b.Property(x => x.Email).HasColumnName("email").HasMaxLength(120);
        b.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(40).IsRequired();
        b.Property(x => x.Notas).HasColumnName("notas");
        b.Property(x => x.Activo).HasColumnName("activo").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("created_at")
        .HasColumnType("timestamp without time zone").IsRequired();
    }
}