﻿using BookStore.Domain.Interfaces;
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

        public async Task<Book> Add(Book book)
        {
            var isBookExist = (await _bookRepository.Search(b => b.Name == book.Name)).Any();
            if (isBookExist)
                return null;

            await _bookRepository.Add(book);
            return book;
        }

        public async Task<Book> Update(Book book)
        {
            var isInvalidId = (await _bookRepository.Search(b => b.Name == book.Name && b.Id != book.Id)).Any();
            if (isInvalidId)
                return null;

            await _bookRepository.Update(book);
            return book;
        }

        public async Task<bool> Remove(Book book)
        {
            await _bookRepository.Remove(book);
            return true;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _bookRepository.GetAll();
        }

        public async Task<Book> GetById(int id)
        {
            return await _bookRepository.GetById(id);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategory(int categoryId)
        {
            return await _bookRepository.GetBooksByCategory(categoryId);
        }

        public async Task<IEnumerable<Book>> Search(string bookName)
        {
            return await _bookRepository.Search(b => b.Name.Contains(bookName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<IEnumerable<Book>> SearchBookWithCategory(string searchedValue)
        {
            return await _bookRepository.SearchBookWithCategory(searchedValue);
        }

        public void Dispose()
        {
            _bookRepository?.Dispose();
        }
    }
}