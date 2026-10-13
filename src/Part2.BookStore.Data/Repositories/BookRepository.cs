using BookStore.Data.Entities;
using BookStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories;

public class BookRepository(BookStoreContext context) : GenericRepository<Book>(context), IBookRepository
{
    public IEnumerable<Book> GetByAuthor(int authorId) =>
        Set.AsNoTracking().Where(b => b.AuthorId == authorId).OrderBy(b => b.Year).ToList();

    public IEnumerable<Book> GetByPriceRange(decimal minPrice, decimal maxPrice) =>
        Set.AsNoTracking().Include(b => b.Author)
            .Where(b => b.Price >= minPrice && b.Price <= maxPrice)
            .OrderBy(b => b.Price)
            .ToList();

    public IEnumerable<Book> SearchByTitle(string text) =>
        Set.AsNoTracking().Include(b => b.Author)
            .Where(b => EF.Functions.Like(b.Title, $"%{text}%"))
            .ToList();
}
