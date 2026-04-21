using JoyitasChirinos.Domain.Common;
using JoyitasChirinos.Domain.Enums;

namespace JoyitasChirinos.Domain.Entities;

public class CajaMovimiento : BaseEntity 
{
    public Guid CajaSesionId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public TipoMovimientoCaja Tipo { get; private set; }
    public decimal Monto { get; private set; }
    public string Motivo { get; private set; } = string.Empty;
    public string? Observaciones { get; private set; }
    public DateTime FechaMovimiento { get; private set; } = DateTime.Now;
    public CajaSesion CajaSesion { get; private set; } = default!;

    protected CajaMovimiento() { }

    public CajaMovimiento(Guid cajaSesionId, Guid usuarioId, TipoMovimientoCaja tipo, decimal monto, string motivo, string? observaciones = null)
    {
        if (cajaSesionId == Guid.Empty) throw new ArgumentException("La caja es obligatoria.", nameof(cajaSesionId));
        if (usuarioId == Guid.Empty) throw new ArgumentException("El usuario es obligatorio.", nameof(usuarioId));
        if (monto <= 0) throw new ArgumentException("El monto debe ser mayor a 0.", nameof(monto));
        if (string.IsNullOrWhiteSpace(motivo)) throw new ArgumentException("El motivo es obligatorio.", nameof(motivo));
        
        CajaSesionId = cajaSesionId;
        UsuarioId = usuarioId;
        Tipo = tipo;
        Monto = monto;
        Motivo = motivo.Trim();
        Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim();
    }
}