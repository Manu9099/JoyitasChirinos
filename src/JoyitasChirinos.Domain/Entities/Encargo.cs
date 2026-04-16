using JoyitasChirinos.Domain.Common;
using JoyitasChirinos.Domain.Enums;
using JoyitasChirinos.Domain.ValueObjects;
namespace JoyitasChirinos.Domain.Entities;
public class Encargo : AuditableEntity
{
    public int Numero { get; private set; }
    public Guid ClienteId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;
    public MaterialProducto Material { get; private set; }
    public PesoGramos? PesoEstimado { get; private set; }
    public Dinero PrecioAcordado { get; private set; } = Dinero.Cero;
    public Dinero Adelanto { get; private set; } = Dinero.Cero;
    public Dinero SaldoPendiente => new(PrecioAcordado.Monto - Adelanto.Monto);
    public EstadoEncargo Estado { get; private set; } = EstadoEncargo.Pendiente;
    public DateTime? FechaEntrega { get; private set; }
    public string? FotoReferenciaUrl { get; private set; }
    public string? Notas { get; private set; }
    public Cliente? Cliente { get; private set; }
    protected Encargo() { }
    public static Encargo Crear(Guid clienteId, Guid usuarioId, string descripcion,
        MaterialProducto material, decimal precioAcordado, decimal adelanto = 0,
        DateTime? fechaEntrega = null, decimal? pesoEstimado = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(descripcion);
        if (adelanto > precioAcordado) throw new ArgumentException("Adelanto supera el precio");
        return new Encargo
        {
            ClienteId = clienteId, UsuarioId = usuarioId, Descripcion = descripcion.Trim(),
            Material = material, PrecioAcordado = new Dinero(precioAcordado),
            Adelanto = new Dinero(adelanto), FechaEntrega = fechaEntrega,
            PesoEstimado = pesoEstimado.HasValue ? new PesoGramos(pesoEstimado.Value) : null
        };
    }
    public void AvanzarEstado()
    {
        Estado = Estado switch
        {
            EstadoEncargo.Pendiente    => EstadoEncargo.EnProduccion,
            EstadoEncargo.EnProduccion => EstadoEncargo.Listo,
            EstadoEncargo.Listo        => EstadoEncargo.Entregado,
            _ => throw new InvalidOperationException($"No se puede avanzar desde {Estado}")
        };
        Touch();
    }
    public void RegistrarAdelanto(decimal monto)
    {
        if (monto <= 0) throw new ArgumentException("Monto debe ser positivo");
        if (Adelanto.Monto + monto > PrecioAcordado.Monto) throw new InvalidOperationException("Supera el precio acordado");
        Adelanto = new Dinero(Adelanto.Monto + monto); Touch();
    }
}
