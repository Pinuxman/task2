namespace BookStore.Data.Interfaces;

/// <summary>Обобщённый репозиторий: базовые CRUD-операции над сущностью.</summary>
public interface IRepository<T> where T : class
{
    T? GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

/// <summary>Асинхронные варианты тех же операций.</summary>
public interface IAsyncRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
