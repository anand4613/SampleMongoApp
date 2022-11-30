using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.Collections;
using SampleMongoApp.Interfaces;
using SampleMongoApp.Entities;
using Microsoft.Extensions.Logging;

namespace SampleMongoApp.Services
{
    public class MongoRepository<T> : IMongoRepository<T> where T : IEntity
    {
        private readonly Mongosettings mongosettings;

        private readonly ILogger<MongoRepository<T>> logger;

        #region Constructors

        protected internal IMongoCollection<T> collection;

        public MongoRepository(IOptions<Mongosettings> mongosetting, ILogger<MongoRepository<T>> logger)
        {
            this.logger = logger;
            this.mongosettings = mongosetting.Value;
            var connectionString = mongosettings.Connection;
            this.collection = ConnectionInfo.GetCollectionFromConnectionString<T>(connectionString);          
        }

        public MongoRepository(string connectionString)
        {
         this.collection = ConnectionInfo.GetCollectionFromConnectionString<T>(connectionString);
        }

        public MongoRepository(string connectionString, string collectionName)
        {
          this.collection = ConnectionInfo.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        #endregion

        #region Methods

        public async Task<T> Add(T entity)
        {
            try
            {
                await this.collection.InsertOneAsync(entity);
            }
            catch (MongoException e)
            {
                if (e.Message.Contains("E11000") || e.Message.ToLower().Contains("duplicate key"))
                {
                    throw new MongoException(string.Format("Mongo Entity '{0}'", entity));
                }
                throw;
            }
            return entity;
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await this.collection.InsertManyAsync(entities);
        }

        public async Task<IQueryable<T>> All()
        {
            return this.collection.AsQueryable(); ;
        }

        public async Task<IQueryable<T>> All(Expression<Func<T, bool>> criteria)
        {
            return this.collection.AsQueryable().Where(criteria);
        }

        public virtual async Task<T> GetById(string id)
        {
            return await this.GetBy(f => f.Id == id);
        }

        public virtual async Task<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return await this.collection.AsQueryable<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IQueryable<T>> AllPages(int page, int pageSize)
        {
            return await Page(All().Result, page, pageSize);
        }

        public IAggregateFluent<T> Aggregation(AggregateOptions options=null)
        {
            return this.collection.Aggregate(options);
        }

        private static async Task<IQueryable<T>> Page(IQueryable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public virtual async Task<T> Update(T entity)
        {
            var result = await this.collection.ReplaceOneAsync<T>((x => x.Id == entity.Id), entity, new ReplaceOptions { IsUpsert = false });
            if (!result.IsModifiedCountAvailable)
            {
                string message = string.Format("Unable to save entity: {0} -- Message: Unable to update Data.", entity.ToJson());
                throw new MongoException(message);
            }

            return entity;
        }

        public virtual async Task Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await this.Update(entity);
            }
        }

        public async Task Delete(string id)
        {
            await collection.DeleteOneAsync(entity => entity.Id == id);
        }


        #endregion

        #region IQueryable<T>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.collection.AsQueryable<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.collection.AsQueryable<T>().GetEnumerator();
        }

        public virtual Type ElementType
        {
            get { return this.collection.AsQueryable<T>().ElementType; }
        }

        public virtual Expression Expression
        {
            get { return this.collection.AsQueryable<T>().Expression; }
        }
    
        public virtual IQueryProvider Provider
        {
            get { return this.collection.AsQueryable<T>().Provider; }
        }

        #endregion
    }
}