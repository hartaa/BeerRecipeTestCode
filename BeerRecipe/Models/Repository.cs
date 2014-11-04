using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;

namespace BeerRecipe.Models
{
    public class Repository<T> : IRepository<T>
    {
        private const string ConnectionString = "mongodb://localhost";

        //Leaving these as private member vars instead of locals in the constructor
        //in case there's a need later.
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _db;
        private readonly MongoCollection<T> _collection;

        protected Repository()
        {
            _client = new MongoClient(ConnectionString);
            _server = _client.GetServer();
            _db = _server.GetDatabase("test");
            _collection = _db.GetCollection<T>(typeof (T).Name);
        }

        public T Save(T document)
        {
            _collection.Save(document);
            return document;
        }

        public T Update(T document)
        {
            _collection.Save(document);
            return document;
        }
        
        public void Remove(string id)
        {
            var query = Query.EQ("_id", id);
            _collection.Remove(query);
        }

        public IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public T GetById(string id)
        {
            return _collection.FindOneById(id);
        }

        public void SaveRange(IEnumerable<T> documents)
        {
            _collection.InsertBatch(documents);
        }
    }
}