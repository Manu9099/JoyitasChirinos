using JoyitasChirinos.Domain.Common;

namespace JoyitasChirinos.Domain.Entities;

public class Proveedor : BaseEntity
{
    public string Nombre { get; private set; } = string.Empty;
    public string? Telefono { get; private set; }
    public string? Email { get; private set; }
    public string Tipo { get; private set; } = "joyeria";
    public string? Notas { get; private set; }
    public bool Activo { get; private set; } = true;

    protected Proveedor() { }

    public static Proveedor Crear(string nombre, string? telefono, string? email, string tipo, string? notas = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);

        return new Proveedor
        {
            Nombre = nombre.Trim(),
            Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
            Tipo = string.IsNullOrWhiteSpace(tipo) ? "joyeria" : tipo.Trim(),
            Notas = string.IsNullOrWhiteSpace(notas) ? null : notas.Trim()
        };
    }

    public void Actualizar(string nombre, string? telefono, string? email, string tipo, string? notas)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);

        Nombre = nombre.Trim();
        Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        Tipo = string.IsNullOrWhiteSpace(tipo) ? "joyeria" : tipo.Trim();
        Notas = string.IsNullOrWhiteSpace(notas) ? null : notas.Trim();
    }

    public void Desactivar() => Activo = false;
    public void Activar() => Activo = true;
}
