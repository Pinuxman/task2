using BookStore.Data.Entities;

namespace BookStore.Data.Interfaces;

public interface IBookRepository : IRepository<Book>, IAsyncRepository<Book>
{
    IEnumerable<Book> GetByAuthor(int authorId);
    IEnumerable<Book> GetByPriceRange(decimal minPrice, decimal maxPrice);
    IEnumerable<Book> SearchByTitle(string text);
}

public interface ICustomerRepository : IRepository<Customer>, IAsyncRepository<Customer>
{
    Customer? GetByEmail(string email);
}

public interface IOrderRepository : IRepository<Order>, IAsyncRepository<Order>
{
    IEnumerable<Order> GetByCustomer(int customerId);
    decimal GetTotalSpent(int customerId);
}
