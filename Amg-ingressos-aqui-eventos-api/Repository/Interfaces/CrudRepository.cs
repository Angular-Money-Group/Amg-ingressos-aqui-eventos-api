using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public class CrudRepository<T> : ICrudRepository<T>
    {
        private readonly IMongoCollection<T> _TCollection;

        public CrudRepository(IDbConnection dbConnection)
        {
            _TCollection = dbConnection.GetConnection<T>();
        }
        public async Task<bool> Delete(string id)
        {
            if (id == null || string.IsNullOrEmpty(id))
                throw new DeleteException("id é obrigatório");

            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            var result = await _TCollection.DeleteOneAsync(filter);

            if (result.DeletedCount == 1)
                return true;
            else
                throw new DeleteException("Algo deu errado ao deletar");
        }

        public Task<bool> Edit(string id, T model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAll()
        {
            var pResult = await _TCollection.Find(_ => true)
                 .As<T>()
                 .ToListAsync();

            return pResult;
        }

        public async Task<List<T>> GetByFilter(Dictionary<string, object> filters)
        {
            var listFilter = filters
                .Select(f => Builders<T>.Filter.Eq(f.Key.ToString(), f.Value)).ToList();
            var builders = Builders<T>.Filter.And(listFilter);
            var result = await _TCollection.Find(builders)
                .As<T>()
                .ToListAsync();

            return result;
        }

        public Task<List<T>> GetByFilter(Dictionary<string, string> filters)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetById(string id)
        {
            if (id == null || string.IsNullOrEmpty(id.ToString()))
                throw new GetException("id é obrigatório");

            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            var variants = await _TCollection.Find(filter).FirstOrDefaultAsync();

            var result = variants ?? throw new GetException("Variant não encontrada");

            return result;
        }

        public async Task<T> Save(T model)
        {
            await _TCollection.InsertOneAsync(model);
            return model;
        }
    }
}