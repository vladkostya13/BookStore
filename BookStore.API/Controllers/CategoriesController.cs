using AutoMapper;
using BookStore.API.Dtos.Category;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CategoryResultDto>>> GetAll()
        {
            var categories = await _categoryService.GetAll();
            var categoriesResultDto = _mapper.Map<IEnumerable<CategoryResultDto>>(categories);

            return Ok(categoriesResultDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<CategoryResultDto>>> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
                return NotFound();

            var categoryResultDto = _mapper.Map<CategoryResultDto>(category);
            return Ok(categoryResultDto);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<CategoryResultDto>>> Add(CategoryAddDto categoryDto)
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<CategoryEditDto>>> Update(int id, CategoryEditDto categoryDto)
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<CategoryResultDto>>> Search(string category)
        {
            var categories = await _categoryService.Search(category);
            if (categories == null || !categories.Any())
                return NotFound("None category was founded");

            var categoriesResultDto = _mapper.Map<List<CategoryResultDto>>(categories);
            return Ok(categoriesResultDto);
        }
    }
}