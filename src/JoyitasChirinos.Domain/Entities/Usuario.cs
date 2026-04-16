using JoyitasChirinos.Domain.Common;
using JoyitasChirinos.Domain.Enums;
namespace JoyitasChirinos.Domain.Entities;
public class Usuario : BaseEntity
{
    public string Nombre { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public RolUsuario Rol { get; private set; }
    public bool Activo { get; private set; } = true;
    protected Usuario() { }
    public static Usuario Crear(string nombre, string email, string passwordHash, RolUsuario rol)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        return new Usuario { Nombre = nombre.Trim(), Email = email.ToLowerInvariant().Trim(), PasswordHash = passwordHash, Rol = rol };
    }
    public void CambiarPassword(string nuevoHash) => PasswordHash = nuevoHash;
    public void Desactivar() => Activo = false;
}
