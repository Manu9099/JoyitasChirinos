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
        b.Property(x => x.UsuarioAperturaId).HasColumnName("usuario_apertura_id").IsRequired();
        b.Property(x => x.UsuarioCierreId).HasColumnName("usuario_cierre_id");
        b.Property(x => x.FechaApertura).HasColumnName("fecha_apertura").HasColumnType("timestamp without time zone").IsRequired();
        b.Property(x => x.FechaCierre).HasColumnName("fecha_cierre").HasColumnType("timestamp without time zone");
        b.Property(x => x.MontoInicial).HasColumnName("monto_inicial").HasColumnType("numeric(10,2)").IsRequired();
        b.Property(x => x.MontoFinalContado).HasColumnName("monto_final_contado").HasColumnType("numeric(10,2)");
        b.Property(x => x.TotalVentasEfectivoCierre).HasColumnName("total_ventas_efectivo_cierre").HasColumnType("numeric(10,2)");
        b.Property(x => x.TotalVentasGeneralCierre).HasColumnName("total_ventas_general_cierre").HasColumnType("numeric(10,2)");
        b.Property(x => x.TotalIngresosManualesCierre).HasColumnName("total_ingresos_manuales_cierre").HasColumnType("numeric(10,2)");
        b.Property(x => x.TotalEgresosManualesCierre).HasColumnName("total_egresos_manuales_cierre").HasColumnType("numeric(10,2)");
        b.Property(x => x.MontoEsperadoCierre).HasColumnName("monto_esperado_cierre").HasColumnType("numeric(10,2)");
        b.Property(x => x.DiferenciaCierre).HasColumnName("diferencia_cierre").HasColumnType("numeric(10,2)");
        b.Property(x => x.EstadoCierre).HasColumnName("estado_cierre").HasMaxLength(20);
        b.Property(x => x.ObservacionesApertura).HasColumnName("observaciones_apertura").HasMaxLength(500);
        b.Property(x => x.ObservacionesCierre).HasColumnName("observaciones_cierre").HasMaxLength(500);
        b.Property(x => x.Abierta).HasColumnName("abierta").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").IsRequired();
        
        b.HasMany(x => x.Movimientos)
            .WithOne(x => x.CajaSesion)
            .HasForeignKey(x => x.CajaSesionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}