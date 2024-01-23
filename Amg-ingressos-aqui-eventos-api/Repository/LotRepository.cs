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
    public class LotRepository : ILotRepository
    {
        private readonly IMongoCollection<Lot> _lotCollection;
        public LotRepository(IDbConnection<Lot> dbConnection)
        {
            _lotCollection = dbConnection.GetConnection("lots");
        }

        public async Task<bool> SaveMany(List<Lot> listLot)
        {
            await _lotCollection.InsertManyAsync(listLot);
            return true;
        }

        public async Task<bool> DeleteByVariant(string idVariant)
        {
            var filter = Builders<Lot>.Filter.Eq(l => l.IdVariant, idVariant.ToString());
            var deleteResult = await _lotCollection.DeleteManyAsync(filter);
            if (deleteResult.DeletedCount >= 1)
                return true;
            else
                throw new DeleteException("algo deu errado ao deletar");
        }

        public async Task<T> GetLotByIdVariant<T>(string idVariant)
        {
            var filtro = Builders<Lot>.Filter.Eq("IdVariant", ObjectId.Parse(idVariant));
            var pResult = await _lotCollection.Find(filtro)
                .As<T>()
                .FirstOrDefaultAsync();

            return pResult ?? throw new GetException("Lote não encontrado");
        }

        public async Task<bool>DeleteMany<T>(List<string> listLot)
        {
           var filtro = Builders<Lot>.Filter.In("_id", listLot);

            var deleteResult = await _lotCollection.DeleteManyAsync(filtro);
            if (deleteResult.DeletedCount == 1)
                return true;
            else
                throw new DeleteException("Algo deu errado ao deletar");
        }

        public async Task<Lot> GetById(string id)
        {
            var filter = Builders<Lot>.Filter.Eq("_Id", ObjectId.Parse(id));
            var pResult = await _lotCollection.Find(filter)
                .As<Lot>()
                .FirstOrDefaultAsync();

            return pResult;
        }

        public async Task<List<Lot>> GetAll()
        {
            var pResult = await _lotCollection.Find(_ => true)
                .As<Lot>()
                .ToListAsync();

            return pResult;
        }

        public async Task<List<Lot>> GetByFilter(Dictionary<string, string> filters)
        {
            var listFilter = new List<FilterDefinition<Lot>>();
            foreach (var item in filters)
                listFilter.Add(Builders<Lot>.Filter.Eq(item.Key.ToString(), item.Value));

            var builders = Builders<Lot>.Filter.And(listFilter);
            var pResult = await _lotCollection.Find(builders)
                .As<Lot>()
                .ToListAsync();

            return pResult;
        }

        public async Task<Lot> Save(Lot model)
        {
            await _lotCollection.InsertOneAsync(model);
            return model ?? throw new SaveException("Erro ao salvar Lote");
        }

        public async Task<bool> Edit(string id, Lot model)
        {
            var filtro = Builders<Lot>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Lot>.Update.Combine();

            foreach (var property in typeof(Lot).GetProperties())
            {
                if (property.GetValue(model) != null && property.Name != "_Id")
                    update = update.Set(property.Name, property.GetValue(model));

            }

            await _lotCollection.UpdateOneAsync(filtro, update);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Lot>.Filter.Eq(l => l.Id, id.ToString());
            var deleteResult = await _lotCollection.DeleteOneAsync(filter);
            if (deleteResult.DeletedCount == 1)
                return true;
            else
                throw new DeleteException("algo deu errado ao deletar");
        }

        public async Task<List<Lot>> GetLotByEndDateSales(DateTime dateManagerLots)
        {
            var filter = Builders<Lot>.Filter.And(
                Builders<Lot>.Filter.Gte(x => x.EndDateSales, dateManagerLots.Date),
                Builders<Lot>.Filter.Lt(x => x.EndDateSales, dateManagerLots.AddDays(1).AddSeconds(-1)),
                Builders<Lot>.Filter.Eq("Status",0)
                );

            var pResult = await _lotCollection.Find(filter)
                .As<Lot>()
                .ToListAsync();

            return pResult;
        }

        public async Task<bool> ChangeStatusLot(string id, int statusLot)
        {
            if (id == null || string.IsNullOrEmpty(id.ToString()))
                throw new EditException("id é obrigatório");

            var filtro = Builders<Lot>.Filter.Eq("_id", ObjectId.Parse(id));

            var updateDefination = Builders<Lot>.Update.Set("Status", statusLot);

            var result = await _lotCollection.UpdateOneAsync(filtro, updateDefination);

            if (result.MatchedCount >= 1)
                return true;
            else
                throw new EditException("Algo deu errado ao atualizar status do Lot");
        }

        public async Task<bool> EditCombine(string id, Lot eventObj)
        {
            var filtro = Builders<Lot>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Lot>.Update.Combine();

            foreach (var property in typeof(Event).GetProperties())
            {
                if (property.GetValue(eventObj) != null && property.Name != "_id")
                {
                    update = update.Set(property.Name, property.GetValue(eventObj));
                }
            }

            await _lotCollection.UpdateOneAsync(filtro, update);
            return true;
        }
    }
}