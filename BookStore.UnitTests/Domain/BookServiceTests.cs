using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookStore.UnitTests.Domain
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookService = new BookService(_bookRepositoryMock.Object);
        }

        #region GetAll

        [Fact]
        public async void GetAll_ShouldReturnAListOfBook_WhenBooksExist()
        {
            var books = CreateBookList();

            _bookRepositoryMock.Setup(c => c.GetAllAsync()).ReturnsAsync(books);

            var result = await _bookService.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenBooksDoNotExist()
        {
            _bookRepositoryMock.Setup(c => c.GetAllAsync())
                .ReturnsAsync((List<Book>)null);

            var result = await _bookService.GetAllAsync();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _bookRepositoryMock.Setup(c => c.GetAllAsync())
                .ReturnsAsync(new List<Book>());

            await _bookService.GetAllAsync();

            _bookRepositoryMock.Verify(mock => mock.GetAllAsync(), Times.Once);
        }

        #endregion

        #region GetById

        [Fact]
        public async void GetById_ShouldReturnBook_WhenBookExist()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c => c.GetByIdAsync(book.Id))
                .ReturnsAsync(book);

            var result = await _bookService.GetByIdAsync(book.Id);

            Assert.NotNull(result);
            Assert.IsType<Book>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenBookDoesNotExist()
        {
            _bookRepositoryMock.Setup(c => c.GetByIdAsync(1))
                .ReturnsAsync((Book)null);

            var result = await _bookService.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _bookRepositoryMock.Setup(c => c.GetByIdAsync(1))
                .ReturnsAsync(new Book());

            await _bookService.GetByIdAsync(1);

            _bookRepositoryMock.Verify(mock => mock.GetByIdAsync(1), Times.Once);
        }

        #endregion

        #region GetBooksByCategory

        [Fact]
        public async void GetBooksByCategory_ShouldReturnAListOfBook_WhenBooksWithSearchedCategoryExist()
        {
            var bookList = CreateBookList();

            _bookRepositoryMock.Setup(c => c.GetBooksByCategoryAsync(2))
                .ReturnsAsync(bookList);

            var result = await _bookService.GetBooksByCategoryAsync(2);

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldReturnNull_WhenBooksWithSearchedCategoryDoNotExist()
        {
            _bookRepositoryMock.Setup(c => c.GetBooksByCategoryAsync(2))
                .ReturnsAsync((IEnumerable<Book>)null);

            var result = await _bookService.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetBooksByCategory_ShouldCallGetBooksByCategoryFromRepository_OnlyOnce()
        {
            var bookList = CreateBookList();

            _bookRepositoryMock.Setup(c => c.GetBooksByCategoryAsync(2))
                .ReturnsAsync(bookList);

            await _bookService.GetBooksByCategoryAsync(2);

            _bookRepositoryMock.Verify(mock => mock.GetBooksByCategoryAsync(2), Times.Once);
        }

        #endregion

        #region Search

        [Fact]
        public async void Search_ShouldReturnAListOfBook_WhenBooksWithSearchedNameExist()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();
            var bookName = searchedBook.Name;

            _bookRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name.Contains(bookName, StringComparison.CurrentCultureIgnoreCase))).ReturnsAsync(bookList);

            var result = await _bookService.SearchAsync(searchedBook.Name);

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenBooksWithSearchedNameDoNotExist()
        {
            var searchedBook = CreateBook();
            var bookName = searchedBook.Name;

            _bookRepositoryMock.Setup(c =>
                    c.SearchAsync(c => c.Name.Contains(bookName, StringComparison.CurrentCultureIgnoreCase)))
                .ReturnsAsync((IEnumerable<Book>)(null));

            var result = await _bookService.SearchAsync(searchedBook.Name);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();
            var bookName = searchedBook.Name;

            _bookRepositoryMock.Setup(c =>
                    c.SearchAsync(c => c.Name.Contains(bookName, StringComparison.CurrentCultureIgnoreCase)))
                .ReturnsAsync(bookList);

            await _bookService.SearchAsync(searchedBook.Name);

            _bookRepositoryMock.Verify(mock => mock.SearchAsync(c => c.Name.Contains(bookName, StringComparison.CurrentCultureIgnoreCase)), Times.Once);
        }

        #endregion

        #region SearchBookWithCategory

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnAListOfBook_WhenBooksWithSearchedCategoryExist()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.SearchBookWithCategoryAsync(searchedBook.Name))
                .ReturnsAsync(bookList);

            var result = await _bookService.SearchBookWithCategoryAsync(searchedBook.Name);

            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldReturnNull_WhenBooksWithSearchedCategoryDoNotExist()
        {
            var searchedBook = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.SearchBookWithCategoryAsync(searchedBook.Name))
                .ReturnsAsync((IEnumerable<Book>)null);

            var result = await _bookService.SearchBookWithCategoryAsync(searchedBook.Name);

            Assert.Null(result);
        }

        [Fact]
        public async void SearchBookWithCategory_ShouldCallSearchBookWithCategoryFromRepository_OnlyOnce()
        {
            var bookList = CreateBookList();
            var searchedBook = CreateBook();

            _bookRepositoryMock.Setup(c =>
                    c.SearchBookWithCategoryAsync(searchedBook.Name))
                .ReturnsAsync(bookList);

            await _bookService.SearchBookWithCategoryAsync(searchedBook.Name);

            _bookRepositoryMock.Verify(mock => mock.SearchBookWithCategoryAsync(searchedBook.Name), Times.Once);
        }

        #endregion

        #region Add

        [Fact]
        public async void Add_ShouldAddBook_WhenBookNameDoesNotExist()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name == book.Name))
                .ReturnsAsync(new List<Book>());
            _bookRepositoryMock.Setup(c => c.AddAsync(book));

            var result = await _bookService.AddAsync(book);

            Assert.NotNull(result);
            Assert.IsType<Book>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddBook_WhenBookNameAlreadyExist()
        {
            var book = CreateBook();
            var bookList = new List<Book>() { book };

            _bookRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name == book.Name))
                .ReturnsAsync(bookList);

            var result = await _bookService.AddAsync(book);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                    c.SearchAsync(c => c.Name == book.Name))
                .ReturnsAsync(new List<Book>());
            _bookRepositoryMock.Setup(c => c.AddAsync(book));

            await _bookService.AddAsync(book);

            _bookRepositoryMock.Verify(mock => mock.AddAsync(book), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_ShouldUpdateBook_WhenBookNameDoesNotExist()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name == book.Name && c.Id != book.Id))
                .ReturnsAsync(new List<Book>());
            _bookRepositoryMock.Setup(c => c.UpdateAsync(book));

            var result = await _bookService.UpdateAsync(book);

            Assert.NotNull(result);
            Assert.IsType<Book>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateBook_WhenBookDoesNotExist()
        {
            var book = CreateBook();
            var bookList = new List<Book>()
            {
                new Book()
                {
                    Id = 2,
                    Name = "Book Test 2",
                    Author = "Author Test 2"
                }
            };

            _bookRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name == book.Name && c.Id != book.Id))
                .ReturnsAsync(bookList);

            var result = await _bookService.UpdateAsync(book);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallAddFromRepository_OnlyOnce()
        {
            var book = CreateBook();

            _bookRepositoryMock.Setup(c =>
                    c.SearchAsync(c => c.Name == book.Name && c.Id != book.Id))
                .ReturnsAsync(new List<Book>());

            await _bookService.UpdateAsync(book);

            _bookRepositoryMock.Verify(mock => mock.UpdateAsync(book), Times.Once);
        }

        #endregion

        #region Remove

        [Fact]
        public async void Remove_ShouldReturnTrue_WhenBookCanBeRemoved()
        {
            var book = CreateBook();

            var result = await _bookService.RemoveAsync(book);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var book = CreateBook();

            await _bookService.RemoveAsync(book);

            _bookRepositoryMock.Verify(mock => mock.RemoveAsync(book), Times.Once);
        }

        #endregion Remove

        #region Data Mock

        private Book CreateBook()
        {
            return new Book()
            {
                Id = 1,
                Name = "Book Test",
                Author = "Author Test",
                Description = "Description Test",
                Price = 10,
                CategoryId = 1,
                PublishDate = DateTime.MinValue.AddYears(40)
            };
        }

        private List<Book> CreateBookList()
        {
            return new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Name = "Book Test 1",
                    Author = "Author Test 1",
                    Description = "Description Test 1",
                    Price = 10,
                    CategoryId = 1,
                    PublishDate = DateTime.MinValue.AddYears(60)
                },
                new Book()
                {
                    Id = 2,
                    Name = "Book Test 2",
                    Author = "Author Test 2",
                    Description = "Description Test 2",
                    Price = 20,
                    CategoryId = 1,
                    PublishDate = DateTime.MinValue.AddYears(80)
                },
                new Book()
                {
                    Id = 3,
                    Name = "Book Test 3",
                    Author = "Author Test 3",
                    Description = "Description Test 3",
                    Price = 30,
                    CategoryId = 2,
                    PublishDate = DateTime.MinValue.AddYears(100)
                }
            };
        }

        #endregion
    }
}