using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;

public class CajaMovimientoConfiguration : IEntityTypeConfiguration<CajaMovimiento> 
{
    public void Configure(EntityTypeBuilder<CajaMovimiento> b) 
    {
        b.ToTable("caja_movimientos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.CajaSesionId).HasColumnName("caja_sesion_id").IsRequired();
        b.Property(x => x.UsuarioId).HasColumnName("usuario_id").IsRequired();
        b.Property(x => x.Tipo).HasColumnName("tipo").HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.Monto).HasColumnName("monto").HasColumnType("numeric(10,2)").IsRequired();
        b.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(150).IsRequired();
        b.Property(x => x.Observaciones).HasColumnName("observaciones").HasMaxLength(500);
        b.Property(x => x.FechaMovimiento).HasColumnName("fecha_movimiento").HasColumnType("timestamp without time zone").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").IsRequired();
    }
}