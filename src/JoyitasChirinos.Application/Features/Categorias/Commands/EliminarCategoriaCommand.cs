using MediatR;

namespace JoyitasChirinos.Application.Features.Categorias.Commands;

public record EliminarCategoriaCommand(Guid Id) : IRequest;