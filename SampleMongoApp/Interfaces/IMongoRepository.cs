using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using SampleMongoApp.Entities;

namespace SampleMongoApp.Interfaces
{
    public interface IMongoRepository<T> : IQueryable<T> where T : IEntity
    {
        
        Task<T> Add(T entity);
        Task Add(IEnumerable<T> entities);
        Task<IQueryable<T>> All();
        Task<IQueryable<T>> All(Expression<Func<T, bool>> criteria);
        Task<T> GetById(string id);
        Task<T> GetBy(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> AllPages(int page, int pageSize);
        IAggregateFluent<T> Aggregation(AggregateOptions options = null);
        Task<T> Update(T entity);
        Task Update(IEnumerable<T> entities);
        Task Delete(string id);
       
    }
}
