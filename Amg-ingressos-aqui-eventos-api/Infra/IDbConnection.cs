using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Infra
{
    public interface IDbConnection<T>
    {
        IMongoCollection<T> GetConnection(string colletionName);
        MongoClient GetClient();
    }
}