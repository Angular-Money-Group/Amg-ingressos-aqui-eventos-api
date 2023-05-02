using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class VariantRepository<T> : IVariantRepository
    {
        private readonly IMongoCollection<Variant> _variantCollection;
        public VariantRepository(IDbConnection<Variant> dbconnection)
        {
            _variantCollection = dbconnection.GetConnection("variants");
        }

        public async Task<object> Save<T>(object variant)
        {
            try
            {
                await _variantCollection.InsertOneAsync(variant as Variant);
                return ((Variant)variant).Id;
            }
            catch (SaveVariantException ex)
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