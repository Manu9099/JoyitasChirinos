using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using JoyitasChirinos.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
namespace JoyitasChirinos.Infrastructure.Services.Storage;
public class CloudinaryStorageService : IStorageService
{
    private readonly Cloudinary _cloudinary;
    public CloudinaryStorageService(IConfiguration config)
    {
        var account = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]);
        _cloudinary = new Cloudinary(account);
    }
    public async Task<string> SubirFotoAsync(Stream stream, string nombreArchivo, CancellationToken ct = default)
    {
        var upload = new ImageUploadParams
        {
            File = new FileDescription(nombreArchivo, stream),
            Folder = "joyitas_chirinos/productos"
        };
        var result = await _cloudinary.UploadAsync(upload);
        return result.SecureUrl.ToString();
    }
    public async Task EliminarFotoAsync(string url, CancellationToken ct = default)
    {
        var publicId = url.Split('/').Last().Split('.').First();
        await _cloudinary.DestroyAsync(new DeletionParams($"joyitas_chirinos/productos/{publicId}"));
    }
}
