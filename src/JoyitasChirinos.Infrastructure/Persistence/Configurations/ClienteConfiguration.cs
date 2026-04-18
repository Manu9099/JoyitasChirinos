using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> b)
    {
        b.ToTable("clientes");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .HasColumnName("id");

        b.Property(x => x.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(150)
            .IsRequired();

        b.Property(x => x.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(20);

        b.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(120);

        b.Property(x => x.Dni)
            .HasColumnName("dni")
            .HasMaxLength(20);

        b.Property(x => x.PuntosFidelidad)
            .HasColumnName("puntos_fidelidad")
            .HasDefaultValue(0)
            .IsRequired();

        b.Property(x => x.Notas)
            .HasColumnName("notas")
            .HasMaxLength(500);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone");

        b.HasIndex(x => x.Dni).IsUnique(false);
        b.HasIndex(x => x.Email).IsUnique(false);
    }
}