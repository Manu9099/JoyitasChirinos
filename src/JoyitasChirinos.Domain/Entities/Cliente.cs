using JoyitasChirinos.Domain.Common;
namespace JoyitasChirinos.Domain.Entities;
public class Cliente : BaseEntity
{
    public string Nombre { get; private set; } = string.Empty;
    public string? Telefono { get; private set; }
    public string? Email { get; private set; }
    public string? Dni { get; private set; }
    public int PuntosFidelidad { get; private set; }
    public string? Notas { get; private set; }
    protected Cliente() { }
    public static Cliente Crear(string nombre, string? telefono = null, string? email = null, string? dni = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);
        return new Cliente { Nombre = nombre.Trim(), Telefono = telefono?.Trim(), Email = email?.Trim(), Dni = dni?.Trim() };
    }
    public void AgregarPuntos(int puntos) { if (puntos > 0) PuntosFidelidad += puntos; }
    public void CanjearPuntos(int puntos)
    {
        if (puntos > PuntosFidelidad) throw new InvalidOperationException("Puntos insuficientes");
        PuntosFidelidad -= puntos;
    }
}
