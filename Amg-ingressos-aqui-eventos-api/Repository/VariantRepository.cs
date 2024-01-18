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
    public class VariantRepository : IVariantRepository
    {
        private readonly IMongoCollection<Variant> _variantCollection;

        public VariantRepository(IDbConnection dbconnection)
        {
            _variantCollection = dbconnection.GetConnection<Variant>("variants");
        }

        public async Task<Variant> Save(Variant variant)
        {
            var data = variant ?? throw new SaveException("Variante não pode ser null.");
            await _variantCollection.InsertOneAsync(data);

            return data;
        }

        public async Task<List<Variant>> SaveMany(List<Variant> listVariant)
        {
            if (!listVariant.Any())
                throw new SaveException("Variante não pode ser null.");

            await _variantCollection.InsertManyAsync(listVariant);

            return listVariant;
        }

        public async Task<bool> Edit(string id, Variant variant)
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
                return true;
            else
                throw new EditException("Algo deu errado ao atualizar Variant");
        }

        public async Task<bool> Delete(string id)
        {
            if (id == null || string.IsNullOrEmpty(id))
                throw new DeleteException("id é obrigatório");

            var filter = Builders<Variant>.Filter.Eq(l => l.Id, id);
            var result = await _variantCollection.DeleteOneAsync(filter);

            if (result.DeletedCount == 1)
                return true;
            else
                throw new DeleteException("Algo deu errado ao deletar Variant");
        }

        public async Task<bool> DeleteMany(List<string> listVariant)
        {
            if (listVariant == null)
                throw new DeleteException("Lista de Variantes é obrigatória.");

            var filter = Builders<Variant>.Filter.In("_id", listVariant);

            var deleteResult = await _variantCollection.DeleteManyAsync(filter);
            if (deleteResult.DeletedCount >= 1)
                return true;
            else
                throw new DeleteException("Algo deu errado ao deletar");
        }

        public async Task<Variant> GetById(string id)
        {
            if (id == null || string.IsNullOrEmpty(id.ToString()))
                throw new GetException("id é obrigatório");

            var filter = Builders<Variant>.Filter.Eq("_id", ObjectId.Parse(id));
            var variants = await _variantCollection.Find(filter).FirstOrDefaultAsync();

            var result = variants ?? throw new GetException("Variant não encontrada");

            return result;
        }

        public async Task<List<Variant>> GetAll()
        {
            var pResult = await _variantCollection.Find(_ => true)
                 .As<Variant>()
                 .ToListAsync();

            return pResult;
        }

        public Task<List<Variant>> GetByFilter(Dictionary<string, string> filters)
        {
            throw new NotImplementedException();
        }
    }
}