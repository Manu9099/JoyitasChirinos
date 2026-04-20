using JoyitasChirinos.Application.Features.Categorias.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Categorias.Commands;

public record CrearCategoriaCommand(CrearCategoriaDto Datos) : IRequest<Guid>;