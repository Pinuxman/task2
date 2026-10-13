using BookStore.Data.Entities;
using BookStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories;

public class OrderRepository(BookStoreContext context) : GenericRepository<Order>(context), IOrderRepository
{
    public IEnumerable<Order> GetByCustomer(int customerId) =>
        Set.AsNoTracking().Include(o => o.Book)
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o => o.CreatedAt)
            .ToList();

    public decimal GetTotalSpent(int customerId) =>
        Set.Where(o => o.CustomerId == customerId)
            .Select(o => new { o.UnitPrice, o.Quantity })
            .AsEnumerable()
            .Sum(o => o.UnitPrice * o.Quantity);
}
