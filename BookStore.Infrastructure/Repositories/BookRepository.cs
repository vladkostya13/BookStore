using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(BookStoreDbContext context) : base(context) { }

        public override async Task<List<Book>> GetAll()
        {
            return await _db.Books.AsNoTracking().Include(b => b.Category).OrderBy(b => b.Name).ToListAsync();
        }

        public override async Task<Book> GetById(int id)
        {
            return await _db.Books.AsNoTracking().Include(b => b.Category).Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategory(int categoryId)
        {
            return await Search(x => x.Id == categoryId);
        }

        public async Task<IEnumerable<Book>> SearchBookWithCategory(string searchedValue)
        {
            return await _db.Books.AsNoTracking()
                .Include(b => b.Category)
                .Where(b => b.Name.Contains(searchedValue, StringComparison.CurrentCultureIgnoreCase) ||
                            b.Author.Contains(searchedValue, StringComparison.CurrentCultureIgnoreCase) ||
                            b.Description.Contains(searchedValue, StringComparison.CurrentCultureIgnoreCase) ||
                            b.Category.Name.Contains(searchedValue, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }
    }
}