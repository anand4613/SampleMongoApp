using Microsoft.Extensions.Logging;
using SampleMongoApp.Interfaces;
using SampleMongoApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System;

namespace SampleMongoApp.Services
{
    public class BookService:IBookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly IMongoRepository<Book> _mongoRepository;

        public BookService(ILogger<BookService> logger, IMongoRepository<Book> mongoRepository)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
        }

        public async Task<IEnumerable<Book>> GetAll() 
        {
            _logger.LogInformation("Getting all books");
            return await _mongoRepository.All();
        }

        //public async Task<IEnumerable<Book>> GetAllAsync()

        public async Task<Book> AddBook(Book book)
        {
            return await _mongoRepository.Add(book);
        }

        public async Task AddMultipleBooks(IEnumerable<Book> books)
        {
            await _mongoRepository.Add(books);
        }

        public async Task<Book> UpdateBook(Book book)
        {
            return await _mongoRepository.Update(book);
        }

        public async Task DeleteBook(string id)
        {
            await _mongoRepository.Delete(id);
        }

        public async Task<Book> GetBookByName(string name)
        {
            return await _mongoRepository.GetBy(book => book.Name==name);
        }

        public async Task<IEnumerable<Book>> GetPagedList(int page, int pageSize)
        {
            return await _mongoRepository.AllPages(page, pageSize);
        }
        public object GetCategoryCount()
        {
            return _mongoRepository.Aggregation().Group(x => x.Category, g => new { category = g.Key, totalcount = g.Count() }).ToList();
        }
    }
}
