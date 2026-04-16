using JoyitasChirinos.Domain.Entities;
namespace JoyitasChirinos.Domain.Interfaces.Services;
public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}
