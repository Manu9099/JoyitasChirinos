namespace JoyitasChirinos.Domain.Interfaces.Services;
public interface IStorageService
{
    Task<string> SubirFotoAsync(Stream stream, string nombreArchivo, CancellationToken ct = default);
    Task EliminarFotoAsync(string url, CancellationToken ct = default);
}
