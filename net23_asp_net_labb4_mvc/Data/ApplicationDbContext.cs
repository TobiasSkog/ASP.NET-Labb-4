using Microsoft.EntityFrameworkCore;
using net23_asp_net_labb4_mvc.Models;

namespace net23_asp_net_labb4_mvc.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowedBook>().HasKey(bb => new { bb.CustomerId, bb.BookId });

        // This line is used if using identity and if having a overrided OnModelCreating
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BorrowedBook> BorrowedBooks { get; set; }
}
