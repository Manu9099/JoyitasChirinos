using JoyitasChirinos.Domain.Enums;

namespace JoyitasChirinos.Domain.Entities;

public class Encargo
{
    public Guid Id { get; private set; }
    public int Numero { get; private set; }

    public Guid ClienteId { get; private set; }
    public Guid UsuarioId { get; private set; }

    public string Descripcion { get; private set; } = string.Empty;
    public MaterialProducto Material { get; private set; }

    public decimal? PesoEstimado { get; private set; }
    public decimal PrecioAcordado { get; private set; }
    public decimal Adelanto { get; private set; }
    public decimal SaldoPendiente => PrecioAcordado - Adelanto;

    public EstadoEncargo Estado { get; private set; }
    public DateTime? FechaEntrega { get; private set; }
    public string? FotoReferenciaUrl { get; private set; }
    public string? Notas { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Cliente Cliente { get; private set; } = null!;
    public Usuario Usuario { get; private set; } = null!;

    private Encargo() { }

    public Encargo(
        int numero,
        Guid clienteId,
        Guid usuarioId,
        string descripcion,
        MaterialProducto material,
        decimal? pesoEstimado,
        decimal precioAcordado,
        decimal adelanto,
        DateTime? fechaEntrega,
        string? fotoReferenciaUrl,
        string? notas)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción es obligatoria.");

        if (precioAcordado < 0)
            throw new ArgumentException("El precio acordado no puede ser negativo.");

        if (adelanto < 0)
            throw new ArgumentException("El adelanto no puede ser negativo.");

        if (adelanto > precioAcordado)
            throw new ArgumentException("El adelanto no puede ser mayor al precio acordado.");

        Id = Guid.NewGuid();
        ClienteId = clienteId;
        UsuarioId = usuarioId;
        Descripcion = descripcion.Trim();
        Material = material;
        PesoEstimado = pesoEstimado;
        Numero = numero;
        PrecioAcordado = precioAcordado;
        Adelanto = adelanto;
        FechaEntrega = fechaEntrega;
        FotoReferenciaUrl = fotoReferenciaUrl;
        Notas = notas;
        Estado = EstadoEncargo.Pendiente;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void Actualizar(
        string descripcion,
        MaterialProducto material,
        decimal? pesoEstimado,
        decimal precioAcordado,
        decimal adelanto,
        DateTime? fechaEntrega,
        string? fotoReferenciaUrl,
        string? notas)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción es obligatoria.");

        if (precioAcordado < 0)
            throw new ArgumentException("El precio acordado no puede ser negativo.");

        if (adelanto < 0)
            throw new ArgumentException("El adelanto no puede ser negativo.");

        if (adelanto > precioAcordado)
            throw new ArgumentException("El adelanto no puede ser mayor al precio acordado.");

        Descripcion = descripcion.Trim();
        Material = material;
        PesoEstimado = pesoEstimado;
        PrecioAcordado = precioAcordado;
        Adelanto = adelanto;
        FechaEntrega = fechaEntrega;
        FotoReferenciaUrl = fotoReferenciaUrl;
        Notas = notas;
        UpdatedAt = DateTime.Now;
    }

    public void CambiarEstado(EstadoEncargo estado)
    {
        Estado = estado;
        UpdatedAt = DateTime.Now;
    }
}
