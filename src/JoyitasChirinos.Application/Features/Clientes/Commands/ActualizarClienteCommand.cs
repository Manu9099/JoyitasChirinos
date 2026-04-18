using JoyitasChirinos.Application.Features.Clientes.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Clientes.Commands;

public record ActualizarClienteCommand(Guid Id, ActualizarClienteDto Datos) : IRequest;