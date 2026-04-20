using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> b)
    {
        b.ToTable("categorias");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(80)
            .IsRequired();

        b.Property(x => x.Descripcion)
            .HasColumnName("descripcion");

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        b.HasIndex(x => x.Nombre).IsUnique();
    }
}