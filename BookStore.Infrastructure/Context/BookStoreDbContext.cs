using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStore.Infrastructure.Context
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}