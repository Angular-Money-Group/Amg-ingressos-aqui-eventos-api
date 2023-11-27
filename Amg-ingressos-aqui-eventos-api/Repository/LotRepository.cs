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
                    throw new DeleteException("algo deu errado ao deletar");
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> DeleteByVariant<T1>(object idVariant)
        {
            try
            {
                var filter = Builders<Lot>.Filter.Eq(l => l.IdVariant, idVariant.ToString());
                var deleteResult = await _lotCollection.DeleteManyAsync(filter);
                if (deleteResult.DeletedCount >= 1)
                    return "Lote deletado";
                else
                    throw new DeleteException("algo deu errado ao deletar");
            }
            catch
            {
                throw;
            }
        }

        public async Task<Lot> Edit<T1>(string id, Lot lotObj)
        {
            try
            {
                var filtro = Builders<Lot>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Lot>.Update.Combine();

                foreach (var property in typeof(Lot).GetProperties())
                {
                    if (property.GetValue(lotObj) != null && property.Name != "_Id")
                        update = update.Set(property.Name, property.GetValue(lotObj));

                }

                object value = await _lotCollection.UpdateOneAsync(filtro, update);
                return value as Lot ?? throw new EditException("Algo deu errado ao editar Lote");
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> DeleteMany<T1>(List<string> Lot)
        {
            try
            {
                var filtro = Builders<Lot>.Filter.In("_id", Lot);

                var deleteResult = await _lotCollection.DeleteManyAsync(filtro);
                if (deleteResult.DeletedCount == 1)
                    return "Lote deletado";
                else
                    throw new DeleteException("Algo deu errado ao deletar");
            }
            catch
            {
                throw;
            }
        }

        public async Task<Lot> GetLotByIdVariant<T1>(string idVariant)
        {
            try
            {
                var filtro = Builders<Lot>.Filter.Eq("IdVariant", ObjectId.Parse(idVariant));
                var pResult = await _lotCollection.FindAsync(filtro);

                return pResult.FirstOrDefault() ?? throw new GetException("Lote não encontrado");
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> Save<T1>(object Lot)
        {
            try
            {
                var data = Lot as Lot ?? throw new SaveException("Lote não pode ser null");
                await _lotCollection.InsertOneAsync(data);
                return data.Id ?? throw new SaveException("Erro ao salvar Lote");
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> SaveMany<T1>(List<Lot> Lot)
        {
            try
            {
                await _lotCollection.InsertManyAsync(Lot);
                return Lot;
            }
            catch
            {
                throw;
            }
        }
    }
}