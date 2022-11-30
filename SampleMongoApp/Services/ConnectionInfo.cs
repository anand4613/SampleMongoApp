using MongoDB.Driver;
using System;
using SampleMongoApp.Entities;

namespace SampleMongoApp.Services
{
    public class ConnectionInfo
    {
        protected static IMongoDatabase mongoDatabase;

        public ConnectionInfo(string connectionString)
        {
            mongoDatabase = GetDatabaseFromConnectionString(connectionString);
        }

        public static IMongoCollection<T> GetCollection<T>() where T : IEntity
        {
            return mongoDatabase.GetCollection<T>(GetCollectionName<T>());
        }

        public static IMongoCollection<T> GetCollection<T>(string collectionName) where T : IEntity
        {
            return mongoDatabase.GetCollection<T>(collectionName);
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString) where T : IEntity
        {
            return GetCollectionFromConnectionString<T>(connectionString, GetCollectionName<T>());
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString, string collectionName) where T : IEntity
        {
            return GetDatabaseFromUrl(MongoUrl.Create(connectionString)).GetCollection<T>(collectionName);
        }

        private static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url) where T : IEntity
        {
            return GetCollectionFromUrl<T>(url, GetCollectionName<T>());
        }

        private static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url, string collectionName) where T : IEntity
        {
            return GetDatabaseFromUrl(url).GetCollection<T>(collectionName);
        }

        private static IMongoDatabase GetDatabaseFromConnectionString(string connectionString)
        {
            return GetDatabaseFromUrl(MongoUrl.Create(connectionString));
        }

        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var _client = new MongoClient(url);
            return _client.GetDatabase(url.DatabaseName);
        }

        private static string GetCollectionName<T>() where T : IEntity
        {
            string collectionName;
            collectionName = typeof(T).Name;

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }

            return collectionName;
        }
    }
}
