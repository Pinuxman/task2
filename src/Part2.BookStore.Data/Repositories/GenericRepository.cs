using BookStore.Data.Entities;
using BookStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories;

/// <summary>
/// Универсальный репозиторий для любой сущности. Изменения попадают в БД
/// только после вызова Save() у Unit of Work.
/// </summary>
public class GenericRepository<T>(BookStoreContext context) : IRepository<T>, IAsyncRepository<T>
    where T : class, IEntity
{
    protected readonly BookStoreContext Context = context;
    protected readonly DbSet<T> Set = context.Set<T>();

    public T? GetById(int id) => Set.Find(id);

    public IEnumerable<T> GetAll() => Set.AsNoTracking().ToList();

    public void Add(T entity) => Set.Add(entity);

    public void Update(T entity) => Set.Update(entity);

    public void Delete(int id)
    {
        var entity = Set.Find(id);
        if (entity is not null)
            Set.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await Set.FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default) =>
        await Set.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default) =>
        await Set.AddAsync(entity, ct);

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await Set.FindAsync([id], ct);
        if (entity is not null)
            Set.Remove(entity);
    }
}
