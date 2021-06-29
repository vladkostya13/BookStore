using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Mappings
{
    public class BookMapping : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired().HasColumnType("varchar(150)");
            builder.Property(b => b.Author).IsRequired().HasColumnType("varchar(150)");
            builder.Property(b => b.Description).IsRequired().HasColumnType("varchar(350)");
            builder.Property(b => b.Price).IsRequired();
            builder.Property(b => b.PublishDate).IsRequired();
            builder.Property(b => b.CategoryId).IsRequired();
            builder.ToTable("books");
        }
    }
}