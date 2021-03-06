using AutoMapper;
using BookStore.API.Dtos.Book;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [Route("api/books")]
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<BookResultDto>>> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            var booksResultDto = _mapper.Map<IEnumerable<BookResultDto>>(books);

            return Ok(booksResultDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<BookResultDto>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            var bookResultDto = _mapper.Map<BookResultDto>(book);
            return Ok(bookResultDto);
        }

        [HttpGet]
        [Route("get-books-by-category/{categoryId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<BookResultDto>> GetBooksByCategory(int categoryId)
        {
            var books = await _bookService.GetBooksByCategoryAsync(categoryId);
            if (!books.Any())
                return NotFound();

            var booksResultDto = _mapper.Map<IEnumerable<BookResultDto>>(books);
            return Ok(booksResultDto);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<BookResultDto>> Add(BookAddDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var book = _mapper.Map<Book>(bookDto);
            var bookResult = await _bookService.AddAsync(book);
            if (bookResult == null)
                return BadRequest();

            var bookResultDto = _mapper.Map<BookResultDto>(bookResult);
            return Ok(bookResultDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<BookEditDto>> Update(int id, BookEditDto bookDto)
        {
            if (id != bookDto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var book = _mapper.Map<Book>(bookDto);
            await _bookService.UpdateAsync(book);

            return Ok(bookDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Remove(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            await _bookService.RemoveAsync(book);

            return Ok();
        }

        [HttpGet]
        [Route("search/{bookName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<BookResultDto>>> Search(string bookName)
        {
            var books = await _bookService.SearchAsync(bookName);
            if (books == null || !books.Any())
                return NotFound("None book was founded");

            var booksResultDto = _mapper.Map<List<BookResultDto>>(books);
            return Ok(booksResultDto);
        }

        [HttpGet]
        [Route("search-book-with-category/{searchedValue}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<BookResultDto>>> SearchBookWithCategory(string searchedValue)
        {
            var books = await _bookService.SearchBookWithCategoryAsync(searchedValue);
            if (books == null || !books.Any())
                return NotFound("None book was founded");

            var booksResultDto = _mapper.Map<List<Book>>(books);
            return Ok(booksResultDto);
        }
    }
}