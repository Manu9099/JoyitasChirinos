using JoyitasChirinos.Domain.Entities;
using JoyitasChirinos.Domain.Interfaces.Repositories;
namespace JoyitasChirinos.Infrastructure.Persistence.Repositories;
public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IGenericRepository<Producto>?  _productos;
    private IGenericRepository<Categoria>? _categorias;
    private IGenericRepository<Proveedor>? _proveedores;
    private IGenericRepository<Cliente>?   _clientes;
    private IGenericRepository<Venta>?     _ventas;
    private IGenericRepository<Encargo>?   _encargos;
    private IGenericRepository<Usuario>?   _usuarios;

    public IGenericRepository<Producto>  Productos  => _productos  ??= new GenericRepository<Producto>(context);
    public IGenericRepository<Categoria> Categorias => _categorias ??= new GenericRepository<Categoria>(context);
    public IGenericRepository<Proveedor> Proveedores=> _proveedores??= new GenericRepository<Proveedor>(context);
    public IGenericRepository<Cliente>   Clientes   => _clientes   ??= new GenericRepository<Cliente>(context);
    public IGenericRepository<Venta>     Ventas     => _ventas     ??= new GenericRepository<Venta>(context);
    public IGenericRepository<Encargo>   Encargos   => _encargos   ??= new GenericRepository<Encargo>(context);
    public IGenericRepository<Usuario>   Usuarios   => _usuarios   ??= new GenericRepository<Usuario>(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);
    public void Dispose() => context.Dispose();
}
