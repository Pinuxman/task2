using BookStore.Data.Entities;
using BookStore.Data.Interfaces;

namespace BookStore.Data.Repositories;

public class CustomerRepository(BookStoreContext context)
    : GenericRepository<Customer>(context), ICustomerRepository
{
    public Customer? GetByEmail(string email) =>
        Set.FirstOrDefault(c => c.Email == email);
}
