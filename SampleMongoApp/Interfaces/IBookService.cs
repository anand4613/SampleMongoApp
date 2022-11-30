using SampleMongoApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleMongoApp.Interfaces
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetAll();
        public Task<Book> AddBook(Book book);
        public Task AddMultipleBooks(IEnumerable<Book> books);
        public Task<Book> UpdateBook(Book book);
        public Task DeleteBook(string id);
        public Task<Book> GetBookByName(string name);
        public Task<IEnumerable<Book>> GetPagedList(int page, int pageSize);
        public object GetCategoryCount();

    }
}
