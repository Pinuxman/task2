using BookStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

/// <summary>Точка входа для проектов, подключающих библиотеку.</summary>
public static class BookStoreFactory
{
    /// <summary>Открывает БД SQLite по строке подключения и создаёт таблицы, если их ещё нет.</summary>
    public static IUnitOfWork Create(string connectionString)
    {
        var options = new DbContextOptionsBuilder<BookStoreContext>()
            .UseSqlite(connectionString)
            .Options;
        var context = new BookStoreContext(options);
        context.Database.EnsureCreated();
        return new UnitOfWork(context);
    }
}
