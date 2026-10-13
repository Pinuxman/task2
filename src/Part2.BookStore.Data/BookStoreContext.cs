using BookStore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class BookStoreContext(DbContextOptions<BookStoreContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(b =>
        {
            b.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            b.Property(x => x.Country).HasMaxLength(80);
        });

        modelBuilder.Entity<Book>(b =>
        {
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            // SQLite не умеет сравнивать decimal, поэтому цена хранится как REAL
            b.Property(x => x.Price).HasConversion<double>();
            b.HasOne(x => x.Author).WithMany(a => a.Books)
                .HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Customer>(b =>
        {
            b.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            b.Property(x => x.Email).HasMaxLength(150).IsRequired();
            b.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.Property(x => x.UnitPrice).HasConversion<double>();
            b.HasOne(x => x.Customer).WithMany(c => c.Orders).HasForeignKey(x => x.CustomerId);
            b.HasOne(x => x.Book).WithMany(k => k.Orders)
                .HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
