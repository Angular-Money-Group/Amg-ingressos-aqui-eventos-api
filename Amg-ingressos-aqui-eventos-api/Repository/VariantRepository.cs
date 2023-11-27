using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;

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

        public async Task<object> Save<T1>(object variant)
        {
            try
            {
                var data = variant as Variant ?? throw new SaveException("Variante não pode ser null.");
                await _variantCollection.InsertOneAsync(data);

                return data.Id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> SaveMany<T1>(List<Variant> listVariant)
        {
            try
            {
                if (listVariant.Any())
                    throw new SaveException("Variante não pode ser null.");

                await _variantCollection.InsertManyAsync(listVariant);
                
                return listVariant;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Variant> Edit<T1>(string id, Variant variant)
        {
            try
            {
                if (id == null || string.IsNullOrEmpty(id.ToString()))
                    throw new EditException("id é obrigatório");
                if (variant == null)
                    throw new EditException("Variante é obrigatório");

                var filtro = Builders<Variant>.Filter.Eq("_id", ObjectId.Parse(id));

                var update = Builders<Variant>.Update
                    .Set("Name", variant.Name)
                    .Set("Description", variant.Description)
                    .Set("HasPositions", variant.HasPositions)
                    .Set("Status", variant.Status)
                    .Set("IdEvent", variant.IdEvent)
                    .Set("QuantityCourtesy", variant.QuantityCourtesy)
                    .Set("ReqDocs", variant.ReqDocs)
                    .Set("Positions", variant.Positions);

                var result = await _variantCollection.UpdateOneAsync(filtro, update);

                if (result.MatchedCount >= 1)
                    return variant;
                else
                    throw new EditException("Algo deu errado ao atualizar Variant");
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> Delete<T1>(object id)
        {
            try
            {
                if (id == null || string.IsNullOrEmpty(id.ToString()))
                    throw new DeleteException("id é obrigatório");

                var filter = Builders<Variant>.Filter.Eq(l => l.Id, id.ToString());
                var result = await _variantCollection.DeleteOneAsync(filter);

                if (result.DeletedCount == 1)
                    return "Variante deletada";
                else
                    throw new DeleteException("Algo deu errado ao deletar Variant");
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> DeleteMany<T1>(List<string> variants)
        {
            try
            {
                if (variants == null)
                    throw new DeleteException("Lista de Variantes é obrigatória.");

                var filter = Builders<Variant>.Filter.In("_id", variants);

                var deleteResult = await _variantCollection.DeleteManyAsync(filter);
                if (deleteResult.DeletedCount >= 1)
                    return "Variantes deletadas";
                else
                    throw new DeleteException("Algo deu errado ao deletar");
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Variant>> FindById<T1>(string id)
        {
            try
            {
                if (id == null || string.IsNullOrEmpty(id.ToString()))
                    throw new GetException("id é obrigatório");

                var filter = Builders<Variant>.Filter.Eq(l => l.Id, id.ToString());

                var pResult = await _variantCollection.FindAsync<Variant>(filter)
                .Result
                .ToListAsync();

                if (!pResult.Any())
                    throw new GetException("Variante não encontrada");

                return pResult;
            }
            catch
            {
                throw;
            }
        }
    }
}