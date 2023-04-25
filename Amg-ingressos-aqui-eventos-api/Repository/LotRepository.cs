using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using System.Data;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

namespace Amg_ingressos_aqui_Lotos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class LotRepository<T> : ILotRepository
    {
        private readonly IMongoCollection<Lot> _variantCollection;
        public LotRepository(IDbConnection<Lot> dbconnection)
        {
            _variantCollection = dbconnection.GetConnection();
        }
        
        public async Task<object> Save<T>(object LotComplet)
        {
            try
            {
                await _variantCollection.InsertOneAsync(LotComplet as Lot);
                return "Lote criado";
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