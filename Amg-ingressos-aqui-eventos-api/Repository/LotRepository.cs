using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using System.Data;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class LotRepository<T> : ILotRepository
    {
        private readonly IMongoCollection<Lot> _lotCollection;
        private readonly MongoClient _mongoClient;
        public LotRepository(IDbConnection<Lot> dbConnection)
        {
            _lotCollection = dbConnection.GetConnection("lots");
            _mongoClient = dbConnection.GetClient();
        }

        public async Task<object> Save<T>(object Lot)
        {
            using (var session = await _mongoClient.StartSessionAsync())
            {
                try
                {
                    await _lotCollection.InsertOneAsync(Lot as Lot);
                    return ((Lot)Lot).Id;
                    await session.CommitTransactionAsync();
                }
                catch (SaveLotException ex)
                {
                    await session.AbortTransactionAsync();
                    throw ex;
                }
                catch (System.Exception ex)
                {
                    await session.AbortTransactionAsync();
                    throw;

                }
            }
        }
        public async Task<object> Delete<T>(object id)
        {
            using (var session = await _mongoClient.StartSessionAsync())
            {
                try
                {
                    var filter = Builders<Lot>.Filter.Eq(l => l.Id, id.ToString());
                    var deleteResult = await _lotCollection.DeleteOneAsync(filter);
                    if (deleteResult.DeletedCount == 1)
                        return "Lote deletado";
                    else
                        throw new SaveLotException("algo deu errado ao deletar");

                    await session.CommitTransactionAsync();
                }
                catch (SaveLotException ex)
                {
                    await session.AbortTransactionAsync();
                    throw ex;
                }
                catch (System.Exception ex)
                {
                    await session.AbortTransactionAsync();
                    throw;

                }
            }
        }
    }
}