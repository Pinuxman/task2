namespace BookStore.Data.Entities;

/// <summary>Общий контракт сущности: наличие целочисленного первичного ключа.</summary>
public interface IEntity
{
    int Id { get; set; }
}

public class Author : IEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Country { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}

public class Book : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Customer : IEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Order : IEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Quantity { get; set; }

    /// <summary>Цена книги на момент покупки.</summary>
    public decimal UnitPrice { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}
