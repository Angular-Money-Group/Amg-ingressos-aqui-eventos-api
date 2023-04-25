using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using System.Data;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

namespace Amg_ingressos_aqui_Variantos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class VariantRepository<T> : IVariantRepository
    {
        private readonly IMongoCollection<Variant> _variantCollection;
        public VariantRepository(IDbConnection<Variant> dbconnection)
        {
            _variantCollection = dbconnection.GetConnection();
        }
        
        public async Task<object> Save<T>(object VariantComplet)
        {
            try
            {
                await _variantCollection.InsertOneAsync(VariantComplet as Variant);
                return "Variante criada";
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