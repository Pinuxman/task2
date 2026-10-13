using Microsoft.EntityFrameworkCore;

namespace UserApi;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("User");
            b.HasKey(x => x.Id);
            b.Property(x => x.Login).HasMaxLength(50).IsRequired();
            b.HasIndex(x => x.Login).IsUnique();
            b.Property(x => x.PassHash).HasMaxLength(64).IsRequired();
        });
    }
}
