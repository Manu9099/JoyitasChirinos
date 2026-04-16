using JoyitasChirinos.Application.Features.Productos.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Productos.Commands;

public record ActualizarProductoCommand(Guid Id, ActualizarProductoDto Datos) : IRequest;
