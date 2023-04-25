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
        private readonly IMongoCollection<Lot> _variantCollection;
        public LotRepository(IDbConnection<Lot> dbConnection)
        {
            _variantCollection = dbConnection.GetConnection("lots");
        }
        
        public async Task<object> Save<T>(object Lot)
        {
            try
            {
                await _variantCollection.InsertOneAsync(Lot as Lot);
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
    }
}