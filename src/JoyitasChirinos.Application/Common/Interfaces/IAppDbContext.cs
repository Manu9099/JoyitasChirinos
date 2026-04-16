using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Producto> Productos { get; }
    DbSet<Categoria> Categorias { get; }
    DbSet<Proveedor> Proveedores { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}