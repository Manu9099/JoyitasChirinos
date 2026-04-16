using MediatR;

namespace JoyitasChirinos.Application.Features.Productos.Commands;

public record SubirFotoProductoCommand(Guid ProductoId, Stream FotoStream, string NombreArchivo) : IRequest<string>;
