using Amg_ingressos_aqui_eventos_api.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Infra
{
    public class DbConnection : IDbConnection
    {
        private IOptions<EventDatabaseSettings> _config;
        public DbConnection(IOptions<EventDatabaseSettings> eventDatabaseSettings)
        {
            _config = eventDatabaseSettings;
        }

        public IMongoCollection<Event> GetConnection(){

            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var _mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = _mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<Event>(_config.Value.EventCollectionName);
        }
    }
}