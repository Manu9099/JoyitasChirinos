using JoyitasChirinos.Domain.Common;

namespace JoyitasChirinos.Domain.Entities;

public class CajaSesion : BaseEntity 
{
    public Guid UsuarioAperturaId { get; private set; }
    public Guid? UsuarioCierreId { get; private set; }
    public DateTime FechaApertura { get; private set; } = DateTime.Now;
    public DateTime? FechaCierre { get; private set; }
    public decimal MontoInicial { get; private set; }
    public decimal? MontoFinalContado { get; private set; }
    public decimal? TotalVentasEfectivoCierre { get; private set; }
    public decimal? TotalVentasGeneralCierre { get; private set; }
    public decimal? TotalIngresosManualesCierre { get; private set; }
    public decimal? TotalEgresosManualesCierre { get; private set; }
    public decimal? MontoEsperadoCierre { get; private set; }
    public decimal? DiferenciaCierre { get; private set; }
    public string? EstadoCierre { get; private set; }
    public string? ObservacionesApertura { get; private set; }
    public string? ObservacionesCierre { get; private set; }
    public bool Abierta { get; private set; } = true;
    public ICollection<CajaMovimiento> Movimientos { get; private set; } = new List<CajaMovimiento>();

    protected CajaSesion() { }

    public CajaSesion(Guid usuarioAperturaId, decimal montoInicial, string? observacionesApertura = null)
    {
        if (usuarioAperturaId == Guid.Empty) throw new ArgumentException("El usuario de apertura es obligatorio.", nameof(usuarioAperturaId));
        if (montoInicial < 0) throw new ArgumentException("El monto inicial no puede ser negativo.", nameof(montoInicial));
        UsuarioAperturaId = usuarioAperturaId;
        MontoInicial = montoInicial;
        ObservacionesApertura = string.IsNullOrWhiteSpace(observacionesApertura) ? null : observacionesApertura.Trim();
    }

    public void Cerrar(Guid usuarioCierreId, decimal montoFinalContado, decimal totalVentasEfectivo, decimal totalVentasGeneral, decimal totalIngresosManuales, decimal totalEgresosManuales, decimal montoEsperado, decimal diferencia, string estadoCierre, string? observacionesCierre = null)
    {
        if (!Abierta) throw new InvalidOperationException("La caja ya está cerrada.");
        if (usuarioCierreId == Guid.Empty) throw new ArgumentException("El usuario de cierre es obligatorio.", nameof(usuarioCierreId));
        if (montoFinalContado < 0) throw new ArgumentException("El monto final contado no puede ser negativo.", nameof(montoFinalContado));
        
        UsuarioCierreId = usuarioCierreId;
        FechaCierre = DateTime.Now;
        MontoFinalContado = montoFinalContado;
        TotalVentasEfectivoCierre = totalVentasEfectivo;
        TotalVentasGeneralCierre = totalVentasGeneral;
        TotalIngresosManualesCierre = totalIngresosManuales;
        TotalEgresosManualesCierre = totalEgresosManuales;
        MontoEsperadoCierre = montoEsperado;
        DiferenciaCierre = diferencia;
        EstadoCierre = estadoCierre;
        ObservacionesCierre = string.IsNullOrWhiteSpace(observacionesCierre) ? null : observacionesCierre.Trim();
        Abierta = false;
    }
}