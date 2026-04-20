using JoyitasChirinos.Application.Features.Proveedores.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Proveedores.Commands;

public record CrearProveedorCommand(CrearProveedorDto Datos) : IRequest<Guid>;