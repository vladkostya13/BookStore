using AutoMapper;
using BookStore.API.Dtos.Category;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            var categoriesResultDto = _mapper.Map<IEnumerable<CategoryResultDto>>(categories);

            return Ok(categoriesResultDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
                return NotFound();

            var categoryResultDto = _mapper.Map<CategoryResultDto>(category);
            return Ok(categoryResultDto);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(CategoryAddDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = _mapper.Map<Category>(categoryDto);
            var categoryResult = await _categoryService.Add(category);
            if (categoryResult == null)
                return BadRequest();

            var categoryResultDto = _mapper.Map<CategoryResultDto>(categoryResult);
            return Ok(categoryResultDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, CategoryEditDto categoryDto)
        {
            if (id != categoryDto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var category = _mapper.Map<Category>(categoryDto);
            await _categoryService.Update(category);

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Remove(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null) 
                return NotFound();

            var result = await _categoryService.Remove(category);
            if (!result) 
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        [Route("search/{category}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<Category>>> Search(string category)
        {
            var categories = await _categoryService.Search(category);
            var categoriesResultDto = _mapper.Map<List<CategoryResultDto>>(categories);
            if (categoriesResultDto == null || categoriesResultDto.Count == 0)
                return NotFound("None category was founded");

            return Ok(categories);
        }
    }
}