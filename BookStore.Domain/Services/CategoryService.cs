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

        public async Task<Category> Add(Category category)
        {
            var isCategoryExist = (await _categoryRepository.Search(c => c.Name == category.Name)).Any();
            if (isCategoryExist)
                return null;

            await _categoryRepository.Add(category);
            return category;
        }

        public async Task<Category> Update(Category category)
        {
            var isInvalidId = (await _categoryRepository.Search(c => c.Name == category.Name && c.Id != category.Id)).Any();
            if (isInvalidId)
                return null;

            await _categoryRepository.Update(category);
            return category;
        }

        public async Task<bool> Remove(Category category)
        {
            var isBooksWithCategoryExist = (await _bookService.GetBooksByCategory(category.Id)).Any();
            if (isBooksWithCategoryExist)
                return false;

            await _categoryRepository.Remove(category);
            return true;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task<IEnumerable<Category>> Search(string categoryName)
        {
            return await _categoryRepository.Search(x => x.Name.Contains(categoryName, StringComparison.CurrentCultureIgnoreCase));
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }
    }
}