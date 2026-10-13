using BookStore.Data.Entities;

namespace BookStore.Data.Interfaces;

/// <summary>Объединяет репозитории одним контекстом и сохраняет их изменения одной транзакцией.</summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Author> Authors { get; }
    IBookRepository Books { get; }
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }

    int Save();
    Task<int> SaveAsync(CancellationToken ct = default);
}
