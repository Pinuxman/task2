using BookStore.Data;
using BookStore.Data.Entities;

// Демонстрация: консольное приложение использует библиотеку BookStore.Data
// и ничего не знает ни об Entity Framework, ни об SQL.
var dbPath = Path.Combine(Path.GetTempPath(), "lab2-bookstore.db");
File.Delete(dbPath);

using var uow = BookStoreFactory.Create($"Data Source={dbPath}");

Console.WriteLine("=== Create: добавление данных ===");
var tolstoy = new Author { FullName = "Лев Толстой", Country = "Россия" };
var martin = new Author { FullName = "Роберт Мартин", Country = "США" };
uow.Authors.Add(tolstoy);
uow.Authors.Add(martin);

uow.Books.Add(new Book { Title = "Война и мир", Year = 1869, Price = 1200m, Stock = 5, Author = tolstoy });
uow.Books.Add(new Book { Title = "Анна Каренина", Year = 1877, Price = 850m, Stock = 3, Author = tolstoy });
uow.Books.Add(new Book { Title = "Чистый код", Year = 2008, Price = 1290m, Stock = 10, Author = martin });
uow.Books.Add(new Book { Title = "Чистая архитектура", Year = 2017, Price = 1490m, Stock = 7, Author = martin });

var customer = new Customer { FullName = "Иван Петров", Email = "petrov@example.com" };
uow.Customers.Add(customer);
Console.WriteLine($"Сохранено записей одной транзакцией: {uow.Save()}");

Console.WriteLine();
Console.WriteLine("=== Read: GetAll и GetById ===");
foreach (var book in uow.Books.GetAll())
    Console.WriteLine($"  #{book.Id} {book.Title} ({book.Year}) – {book.Price} руб., остаток {book.Stock}");
Console.WriteLine($"GetById(3): {uow.Books.GetById(3)?.Title}");
Console.WriteLine($"GetById(99): {uow.Books.GetById(99)?.Title ?? "не найдено"}");

Console.WriteLine();
Console.WriteLine("=== Специализированные методы IBookRepository ===");
Console.WriteLine($"Книги автора «{tolstoy.FullName}»:");
foreach (var book in uow.Books.GetByAuthor(tolstoy.Id))
    Console.WriteLine($"  {book.Title} ({book.Year})");
Console.WriteLine("Книги ценой от 1000 до 1300 руб.:");
foreach (var book in uow.Books.GetByPriceRange(1000m, 1300m))
    Console.WriteLine($"  {book.Title} – {book.Price} руб. ({book.Author.FullName})");
Console.WriteLine("Поиск по названию «Чист»:");
foreach (var book in uow.Books.SearchByTitle("Чист"))
    Console.WriteLine($"  {book.Title}");

Console.WriteLine();
Console.WriteLine("=== Update: изменение цены ===");
var cleanCode = uow.Books.GetById(3)!;
cleanCode.Price = 990m;
uow.Books.Update(cleanCode);
uow.Save();
Console.WriteLine($"Новая цена «{cleanCode.Title}»: {uow.Books.GetById(3)!.Price} руб.");

Console.WriteLine();
Console.WriteLine("=== Unit of Work: заказ меняет две таблицы одной транзакцией ===");
var warAndPeace = uow.Books.GetById(1)!;
uow.Orders.Add(new Order { CustomerId = customer.Id, BookId = warAndPeace.Id, Quantity = 2, UnitPrice = warAndPeace.Price });
warAndPeace.Stock -= 2;
uow.Books.Update(warAndPeace);
uow.Save();
Console.WriteLine($"Остаток «{warAndPeace.Title}» после заказа: {uow.Books.GetById(1)!.Stock}");
foreach (var order in uow.Orders.GetByCustomer(customer.Id))
    Console.WriteLine($"  Заказ #{order.Id}: {order.Book.Title} x{order.Quantity} по {order.UnitPrice} руб.");
Console.WriteLine($"Покупатель {uow.Customers.GetByEmail("petrov@example.com")!.FullName} потратил всего: {uow.Orders.GetTotalSpent(customer.Id)} руб.");

Console.WriteLine();
Console.WriteLine("=== Асинхронные методы ===");
await uow.Books.AddAsync(new Book { Title = "Детство", Year = 1852, Price = 450m, Stock = 4, AuthorId = tolstoy.Id });
await uow.SaveAsync();
var all = await uow.Books.GetAllAsync();
Console.WriteLine($"После AddAsync книг в базе: {all.Count}");
Console.WriteLine($"GetByIdAsync(5): {(await uow.Books.GetByIdAsync(5))?.Title}");

Console.WriteLine();
Console.WriteLine("=== Delete ===");
await uow.Books.DeleteAsync(5);
uow.Books.Delete(2);
uow.Save();
Console.WriteLine($"После удаления книг №5 и №2 осталось: {uow.Books.GetAll().Count()}");
