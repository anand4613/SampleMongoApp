using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleMongoApp.Entities;
using SampleMongoApp.Services;
using SampleMongoApp.Interfaces;

namespace SampleMongoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {

        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }
        
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetAll()
        {
             var response =await _bookService.GetAll();
            return Ok(response);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(Book book)
        {
            var response = await _bookService.AddBook(book);
            return Ok(response);
        }

        [HttpPost]
        [Route("addmany")]
        public async Task<IActionResult> AddMany(IEnumerable<Book> books)
        {
            await _bookService.AddMultipleBooks(books);
            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(Book book)
        {
            var response = await _bookService.UpdateBook(book);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            await _bookService.DeleteBook(id);
            return Ok();
        }

        [HttpGet]
        [Route("getbyname")]
        public async Task<IActionResult> GetByName(string name)
        {
            var response = await _bookService.GetBookByName(name);
            if(response == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet]
        [Route("getpage")]
        public async Task<IActionResult> GetPaged (int page, int pageSize)
        {
            var response = await _bookService.GetPagedList(page, pageSize);
            if (!response.Any())
                return NotFound();
            return Ok(response);
        }
        [HttpGet]
        [Route("getcountbycategory")]
        public IActionResult GetCategoryCount ()
        {
            var response = _bookService.GetCategoryCount();
            return Ok(response);
        }
    }   
}
