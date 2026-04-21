using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class CajaSesionConfiguration : IEntityTypeConfiguration<CajaSesion>
{
    public void Configure(EntityTypeBuilder<CajaSesion> b)
    {
        b.ToTable("caja_sesiones");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        b.Property(x => x.FechaApertura)
            .HasColumnName("fecha_apertura")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        b.Property(x => x.MontoInicial)
            .HasColumnName("monto_inicial")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        b.Property(x => x.FechaCierre)
            .HasColumnName("fecha_cierre")
            .HasColumnType("timestamp without time zone");

        b.Property(x => x.MontoFinal)
            .HasColumnName("monto_final")
            .HasColumnType("numeric(10,2)");

        b.Property(x => x.ObservacionesApertura)
            .HasColumnName("observaciones_apertura")
            .HasMaxLength(500);

        b.Property(x => x.ObservacionesCierre)
            .HasColumnName("observaciones_cierre")
            .HasMaxLength(500);

        b.Property(x => x.Abierta)
            .HasColumnName("abierta")
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .IsRequired();
    }
}