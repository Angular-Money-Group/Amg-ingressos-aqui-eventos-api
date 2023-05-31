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
        public LotRepository(IDbConnection<Lot> dbConnection)
        {
            _lotCollection = dbConnection.GetConnection("lots");
        }

        public async Task<object> Delete<T1>(object id)
        {
            try
            {
                var filter = Builders<Lot>.Filter.Eq(l => l.Id, id.ToString());
                var deleteResult = await _lotCollection.DeleteOneAsync(filter);
                if (deleteResult.DeletedCount == 1)
                    return "Lote deletado";
                else
                    throw new SaveLotException("algo deu errado ao deletar");
            }
            catch (SaveLotException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<object> Save<T>(object Lot)
        {
            try
            {
                await _lotCollection.InsertOneAsync(Lot as Lot);
                return ((Lot)Lot).Id;
            }
            catch (SaveLotException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Lot>> FindByIdVariant<T>(string id)
        {
            try
            {
                var filter = Builders<Lot>.Filter.Eq("IdVariant", id);
                var pResult = _lotCollection.Find(filter).ToList();

                if(!pResult.Any() || pResult.Count == 0)
                    throw new FindLotsByIdVariantException("Lots não encontrados");

                return pResult;
            }
            catch (FindLotsByIdVariantException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}