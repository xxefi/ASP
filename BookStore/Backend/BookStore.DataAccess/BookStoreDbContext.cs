using BookStore.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<BookEntity> Books { get; set; }


}
