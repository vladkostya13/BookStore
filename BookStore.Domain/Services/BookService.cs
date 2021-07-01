using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Domain.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> AddAsync(Book book)
        {
            var isBookExist = (await _bookRepository.SearchAsync(b => b.Name == book.Name)).Any();
            if (isBookExist)
                return null;

            await _bookRepository.AddAsync(book);
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            var isBookExist = (await _bookRepository.SearchAsync(b => b.Name == book.Name && b.Id != book.Id)).Any();
            if (isBookExist)
                return null;

            await _bookRepository.UpdateAsync(book);
            return book;
        }

        public async Task<bool> RemoveAsync(Book book)
        {
            await _bookRepository.RemoveAsync(book);
            return true;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _bookRepository.GetBooksByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Book>> SearchAsync(string bookName)
        {
            return await _bookRepository.SearchAsync(b => b.Name.Contains(bookName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<IEnumerable<Book>> SearchBookWithCategoryAsync(string searchedValue)
        {
            return await _bookRepository.SearchBookWithCategoryAsync(searchedValue);
        }

        public void Dispose()
        {
            _bookRepository?.Dispose();
        }
    }
}