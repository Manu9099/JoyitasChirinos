using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("usuarios");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        b.Property(x => x.Email).HasColumnName("email").HasMaxLength(120).IsRequired();
        b.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
        b.Property(x => x.Rol).HasColumnName("rol").HasConversion<string>().IsRequired();
        b.Property(x => x.Activo).HasColumnName("activo").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasIndex(x => x.Email).IsUnique();
    }
}