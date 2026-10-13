using BookStore.Data;
using BookStore.Data.Entities;
using BookStore.Data.Interfaces;

namespace Lab2.Tests;

public class RepositoryTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lab2-test-{Guid.NewGuid():N}.db");
    private readonly IUnitOfWork _uow;
    private readonly Author _author = new() { FullName = "Роберт Мартин" };

    public RepositoryTests()
    {
        _uow = BookStoreFactory.Create($"Data Source={_dbPath};Pooling=False");
        _uow.Authors.Add(_author);
        _uow.Books.Add(new Book { Title = "Чистый код", Year = 2008, Price = 1290m, Stock = 10, Author = _author });
        _uow.Books.Add(new Book { Title = "Чистая архитектура", Year = 2017, Price = 1490m, Stock = 7, Author = _author });
        _uow.Save();
    }

    public void Dispose()
    {
        _uow.Dispose();
        File.Delete(_dbPath);
    }

    [Fact]
    public void Add_DoesNotReachDatabase_UntilSave()
    {
        _uow.Books.Add(new Book { Title = "Идеальный программист", Price = 900m, AuthorId = _author.Id });
        Assert.Equal(2, _uow.Books.GetAll().Count());

        _uow.Save();
        Assert.Equal(3, _uow.Books.GetAll().Count());
    }

    [Fact]
    public void Update_And_Delete_Work()
    {
        var book = _uow.Books.GetById(1)!;
        book.Price = 990m;
        _uow.Books.Update(book);
        _uow.Books.Delete(2);
        _uow.Save();

        Assert.Equal(990m, _uow.Books.GetById(1)!.Price);
        Assert.Null(_uow.Books.GetById(2));
    }

    [Fact]
    public void GetByPriceRange_ReturnsOnlyBooksInsideRange()
    {
        var books = _uow.Books.GetByPriceRange(1000m, 1300m).ToList();

        Assert.Equal("Чистый код", Assert.Single(books).Title);
    }

    [Fact]
    public void GetByAuthor_And_SearchByTitle_Work()
    {
        Assert.Equal(2, _uow.Books.GetByAuthor(_author.Id).Count());
        Assert.Equal("Чистая архитектура", Assert.Single(_uow.Books.SearchByTitle("архитект")).Title);
    }

    [Fact]
    public async Task AsyncMethods_Work()
    {
        await _uow.Books.AddAsync(new Book { Title = "Идеальный программист", Price = 900m, AuthorId = _author.Id });
        await _uow.SaveAsync();

        Assert.Equal(3, (await _uow.Books.GetAllAsync()).Count);
        Assert.Equal("Идеальный программист", (await _uow.Books.GetByIdAsync(3))!.Title);

        await _uow.Books.DeleteAsync(3);
        await _uow.SaveAsync();
        Assert.Null(await _uow.Books.GetByIdAsync(3));
    }

    [Fact]
    public void UnitOfWork_SavesChangesOfSeveralRepositoriesTogether()
    {
        var customer = new Customer { FullName = "Иван Петров", Email = "petrov@example.com" };
        var book = _uow.Books.GetById(1)!;
        _uow.Customers.Add(customer);
        _uow.Orders.Add(new Order { Customer = customer, BookId = book.Id, Quantity = 2, UnitPrice = book.Price });
        book.Stock -= 2;

        Assert.Equal(3, _uow.Save());

        Assert.Equal(8, _uow.Books.GetById(1)!.Stock);
        Assert.Equal(2580m, _uow.Orders.GetTotalSpent(customer.Id));
        Assert.Equal(customer.Id, _uow.Customers.GetByEmail("petrov@example.com")!.Id);
    }
}
