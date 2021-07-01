using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookService _bookService;

        public CategoryService(ICategoryRepository categoryRepository, IBookService bookService)
        {
            _categoryRepository = categoryRepository;
            _bookService = bookService;
        }

        public async Task<Category> AddAsync(Category category)
        {
            var isCategoryExist = (await _categoryRepository.SearchAsync(c => c.Name == category.Name)).Any();
            if (isCategoryExist)
                return null;

            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var isCategoryExist = (await _categoryRepository.SearchAsync(c => c.Name == category.Name && c.Id != category.Id)).Any();
            if (isCategoryExist)
                return null;

            await _categoryRepository.UpdateAsync(category);
            return category;
        }

        public async Task<bool> RemoveAsync(Category category)
        {
            var isBooksWithCategoryExist = (await _bookService.GetBooksByCategoryAsync(category.Id)).Any();
            if (isBooksWithCategoryExist)
                return false;

            await _categoryRepository.RemoveAsync(category);
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> SearchAsync(string categoryName)
        {
            return await _categoryRepository.SearchAsync(x => x.Name.Contains(categoryName, StringComparison.CurrentCultureIgnoreCase));
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }
    }
}