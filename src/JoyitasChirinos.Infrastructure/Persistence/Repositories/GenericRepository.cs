using JoyitasChirinos.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace JoyitasChirinos.Infrastructure.Persistence.Repositories;
public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _set = context.Set<T>();
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) => await _set.FindAsync([id], ct);
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default) => await _set.ToListAsync(ct);
    public async Task AddAsync(T entity, CancellationToken ct = default) => await _set.AddAsync(entity, ct);
    public void Update(T entity) => _set.Update(entity);
    public void Delete(T entity) => _set.Remove(entity);
}
