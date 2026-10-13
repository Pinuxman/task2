using BookStore.Data.Entities;
using BookStore.Data.Interfaces;
using BookStore.Data.Repositories;

namespace BookStore.Data;

public class UnitOfWork(BookStoreContext context) : IUnitOfWork
{
    public IRepository<Author> Authors { get; } = new GenericRepository<Author>(context);
    public IBookRepository Books { get; } = new BookRepository(context);
    public ICustomerRepository Customers { get; } = new CustomerRepository(context);
    public IOrderRepository Orders { get; } = new OrderRepository(context);

    public int Save() => context.SaveChanges();

    public Task<int> SaveAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
