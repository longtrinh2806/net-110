using Data.MongoCollections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess
{
    public class AppDbContext : DbContext
    {
        private IMongoDatabase _database;
        private IMongoClient _client;

        public AppDbContext(IOptions<AppDatabaseSetting> appDatabaseSetting)
        {
            _client = new MongoClient(appDatabaseSetting.Value.ConnectionString);
            _database = _client.GetDatabase(appDatabaseSetting.Value.DatabaseName);
        }
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("order");
        public IMongoCollection<Shipper> Shippers => _database.GetCollection<Shipper>("shipper");
        public IMongoCollection<Delivery> Deliveries => _database.GetCollection<Delivery>("delivery");
        public IClientSessionHandle StartSession()
        {
            var session = _client.StartSession();
            return session;
        }
        public void CreateCollectionsIfNotExisted()
        {
            var collectionNames = _database.ListCollectionNames().ToList();

            if (!collectionNames.Any(name => name.Equals("order")))
            {
                _database.CreateCollection("order");
            }

            if (!collectionNames.Any(name => name.Equals("shipper")))
            {
                _database.CreateCollection("shipper");
            }

            if (!collectionNames.Any(name => name.Equals("delivery")))
            {
                _database.CreateCollection("delivery");
            }
        }
    }
}
