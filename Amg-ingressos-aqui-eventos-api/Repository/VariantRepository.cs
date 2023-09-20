using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Repository.Querys;

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

        public async Task<object> SaveMany<T>(List<Variant> lstVariant)
        {
            try
            {
                await _variantCollection.InsertManyAsync(lstVariant);
                return lstVariant;
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

        public async Task<object> Delete<T>(object id)
        {
            try
            {
                var filter = Builders<Variant>.Filter.Eq(l => l.Id, id.ToString());

                var deleteResult = await _variantCollection.DeleteOneAsync(filter);
                if (deleteResult.DeletedCount == 1)
                    return "Variante deletada";
                else
                    throw new SaveLotException("Algo deu errado ao deletar");
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

        public async Task<Variant> Edit<T>(string id, Variant variantObj)
        {
            try
            {
                var filtro = Builders<Variant>.Filter.Eq("_id", ObjectId.Parse(id));

                var update = Builders<Variant>.Update
                    .Set("Name", variantObj.Name)
                    .Set("Description", variantObj.Description)
                    .Set("HasPositions", variantObj.HasPositions)
                    .Set("Status", variantObj.Status)
                    .Set("IdEvent", variantObj.IdEvent)
                    .Set("QuantityCourtesy", variantObj.QuantityCourtesy)
                    .Set("ReqDocs", variantObj.ReqDocs)
                    .Set("Positions", variantObj.Positions);

                await _variantCollection.UpdateOneAsync(filtro, update);

                return variantObj;
            }
            catch (SaveEventException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> DeleteMany<T>(List<string> variants)
        {
            try
            {
                var filter = Builders<Variant>.Filter.In("_id", variants);

                var deleteResult = await _variantCollection.DeleteManyAsync(filter);
                if (deleteResult.DeletedCount >= 1)
                    return "Variantes deletadas";
                else
                    throw new SaveLotException("Algo deu errado ao deletar");
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

        public async Task<List<Variant>> FindById<T>(string id)
        {
            try
            {
                var json = QuerysMongo.GetLotsByVariant;

                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument documentFilter = BsonDocument.Parse(
                    @"{ $match: { '_id': '" + id.ToString() + "' }}"
                );
                BsonDocument[] pipeline = new BsonDocument[] { document, documentFilter, };

                var pResults = _variantCollection.Aggregate<Variant>(pipeline).ToList();

                var filter = Builders<Variant>.Filter.Eq(l => l.Id, id.ToString());

                var pResult = _variantCollection.Find<Variant>(filter).ToList();
                if (!pResult.Any())
                    throw new SaveLotException("Variante não encontrada");

                return pResult;
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
    }
}
