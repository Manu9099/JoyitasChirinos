using JoyitasChirinos.Domain.Common;

namespace JoyitasChirinos.Domain.Entities;

public class CajaSesion : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public DateTime FechaApertura { get; private set; } = DateTime.Now;
    public decimal MontoInicial { get; private set; }
    public DateTime? FechaCierre { get; private set; }
    public decimal? MontoFinal { get; private set; }
    public string? ObservacionesApertura { get; private set; }
    public string? ObservacionesCierre { get; private set; }
    public bool Abierta { get; private set; } = true;

    protected CajaSesion() { }

    public CajaSesion(Guid usuarioId, decimal montoInicial, string? observacionesApertura = null)
    {
        if (usuarioId == Guid.Empty)
            throw new ArgumentException("El usuario es obligatorio.", nameof(usuarioId));

        if (montoInicial < 0)
            throw new ArgumentException("El monto inicial no puede ser negativo.", nameof(montoInicial));

        UsuarioId = usuarioId;
        MontoInicial = montoInicial;
        ObservacionesApertura = string.IsNullOrWhiteSpace(observacionesApertura)
            ? null
            : observacionesApertura.Trim();
    }

    public void Cerrar(decimal montoFinal, string? observacionesCierre = null)
    {
        if (!Abierta)
            throw new InvalidOperationException("La caja ya está cerrada.");

        if (montoFinal < 0)
            throw new ArgumentException("El monto final no puede ser negativo.", nameof(montoFinal));

        MontoFinal = montoFinal;
        FechaCierre = DateTime.Now;
        ObservacionesCierre = string.IsNullOrWhiteSpace(observacionesCierre)
            ? null
            : observacionesCierre.Trim();
        Abierta = false;
    }
}