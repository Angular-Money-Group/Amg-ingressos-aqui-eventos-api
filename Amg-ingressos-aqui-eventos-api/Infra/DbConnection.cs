using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Infra
{
    public class DbConnection : IDbConnection
    {
        private readonly IOptions<EventDatabaseSettings> _config;
        private MongoClient _mongoClient;
        public DbConnection(IOptions<EventDatabaseSettings> eventDatabaseSettings)
        {
            _mongoClient = new MongoClient();
            _config = eventDatabaseSettings;
        }

        public IMongoCollection<T> GetConnection<T>(string colletionName)
        {

            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            _mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = _mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<T>(colletionName);
        }
        
        public MongoClient GetClient()
        {
            return _mongoClient;
        }

        public IMongoCollection<T> GetConnection<T>()
        {
            var colletionName = GetCollectionName<T>();
            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            _mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = _mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<T>(colletionName);
        }
        private static string GetCollectionName<T>()
        {

            return typeof(T).Name.ToLower() ?? string.Empty;
        }
    }
}