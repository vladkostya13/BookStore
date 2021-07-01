using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookStore.UnitTests.Domain
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IBookService> _bookService;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _bookService = new Mock<IBookService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _bookService.Object);
        }

        #region GetAll

        [Fact]
        public async void GetAll_ShouldReturnAListOFCategories_WhenCategoriesExist()
        {
            var categories = CreateCategoryList();

            _categoryRepositoryMock.Setup(c =>
                c.GetAllAsync()).ReturnsAsync(categories);

            var result = await _categoryService.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenCategoriesDoNotExist()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetAllAsync()).ReturnsAsync((List<Category>)null);

            var result = await _categoryService.GetAllAsync();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetAllAsync()).ReturnsAsync((List<Category>)null);

            await _categoryService.GetAllAsync();

            _categoryRepositoryMock.Verify(mock => mock.GetAllAsync(), Times.Once);
        }

        #endregion

        #region GetById

        [Fact]
        public async void GetById_ShouldReturnCategory_WhenCategoryExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.GetByIdAsync(category.Id)).ReturnsAsync(category);

            var result = await _categoryService.GetByIdAsync(category.Id);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetByIdAsync(1)).ReturnsAsync((Category)null);

            var result = await _categoryService.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(c =>
                c.GetByIdAsync(1)).ReturnsAsync((Category)null);

            await _categoryService.GetByIdAsync(1);

            _categoryRepositoryMock.Verify(mock => mock.GetByIdAsync(1), Times.Once);
        }

        #endregion

        #region Add

        [Fact]
        public async void Add_ShouldAddCategory_WhenCategoryNameDoesNotExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name == category.Name))
                .ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(c => c.AddAsync(category));

            var result = await _categoryService.AddAsync(category);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddCategory_WhenCategoryNameAlreadyExist()
        {
            var category = CreateCategory();
            var categoryList = new List<Category>() { category };

            _categoryRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name == category.Name)).ReturnsAsync(categoryList);

            var result = await _categoryService.AddAsync(category);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                    c.SearchAsync(c => c.Name == category.Name))
                .ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(c => c.AddAsync(category));

            await _categoryService.AddAsync(category);

            _categoryRepositoryMock.Verify(mock => mock.AddAsync(category), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_ShouldUpdateCategory_WhenCategoryNameDoesNotExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name == category.Name && c.Id != category.Id))
                .ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(c => c.UpdateAsync(category));

            var result = await _categoryService.UpdateAsync(category);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateCategory_WhenCategoryDoesNotExist()
        {
            var category = CreateCategory();
            var categoryList = new List<Category>()
            {
                new Category()
                {
                    Id = 2,
                    Name = "Test Category 2"
                }
            };

            _categoryRepositoryMock.Setup(c =>
                    c.SearchAsync(c => c.Name == category.Name && c.Id != category.Id))
                .ReturnsAsync(categoryList);

            var result = await _categoryService.UpdateAsync(category);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(c =>
                    c.SearchAsync(c => c.Name == category.Name && c.Id != category.Id))
                .ReturnsAsync(new List<Category>());

            await _categoryService.UpdateAsync(category);

            _categoryRepositoryMock.Verify(mock => mock.UpdateAsync(category), Times.Once);
        }

        #endregion

        #region Remove

        [Fact]
        public async void Remove_ShouldRemoveCategory_WhenCategoryDoNotHaveRelatedBooks()
        {
            var category = CreateCategory();

            _bookService.Setup(b =>
                b.GetBooksByCategoryAsync(category.Id)).ReturnsAsync(new List<Book>());

            var result = await _categoryService.RemoveAsync(category);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldNotRemoveCategory_WhenCategoryHasRelatedBooks()
        {
            var category = CreateCategory();
            var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Name = "Test Name 1",
                    Author = "Test Author 1",
                    CategoryId = category.Id
                }
            };

            _bookService.Setup(b => b.GetBooksByCategoryAsync(category.Id)).ReturnsAsync(books);

            var result = await _categoryService.RemoveAsync(category);

            Assert.False(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _bookService.Setup(b =>
                b.GetBooksByCategoryAsync(category.Id)).ReturnsAsync(new List<Book>());

            await _categoryService.RemoveAsync(category);

            _categoryRepositoryMock.Verify(mock => mock.RemoveAsync(category), Times.Once);
        }

        #endregion

        #region Search

        [Fact]
        public async void Search_ShouldReturnAListOfCategory_WhenCategoriesWithSearchedNameExist()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name.Contains(categoryName, StringComparison.CurrentCultureIgnoreCase)))
                .ReturnsAsync(categoryList);

            var result = await _categoryService.SearchAsync(searchedCategory.Name);

            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenCategoriesWithSearchedNameDoNotExist()
        {
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name.Contains(categoryName, StringComparison.CurrentCultureIgnoreCase)))
                .ReturnsAsync((IEnumerable<Category>)(null));

            var result = await _categoryService.SearchAsync(searchedCategory.Name);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(c =>
                c.SearchAsync(c => c.Name.Contains(categoryName, StringComparison.CurrentCultureIgnoreCase)))
                .ReturnsAsync(categoryList);

            await _categoryService.SearchAsync(searchedCategory.Name);

            _categoryRepositoryMock.Verify(mock => mock.SearchAsync(c => c.Name.Contains(categoryName, StringComparison.CurrentCultureIgnoreCase)), Times.Once);
        }

        #endregion

        #region Data Mock

        private Category CreateCategory()
        {
            return new Category()
            {
                Id = 1,
                Name = "Test Category 1"
            };
        }

        private List<Category> CreateCategoryList()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Test Category 1"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Test Category 2"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Test Category 3"
                }
            };
        }

        #endregion
    }
}