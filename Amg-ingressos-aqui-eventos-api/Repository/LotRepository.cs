using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using System.Data;
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
                    throw new SaveLotException("algo deu errado ao deletar");
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

        public async Task<object> DeleteByVariant<T1>(object idVariant)
        {
            try
            {
                var filter = Builders<Lot>.Filter.Eq(l => l.IdVariant, idVariant.ToString());
                var deleteResult = await _lotCollection.DeleteManyAsync(filter);
                if (deleteResult.DeletedCount >= 1)
                    return "Lote deletado";
                else
                    throw new SaveLotException("algo deu errado ao deletar");
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

        public async Task<Lot> Edit<T>(string id, Lot lotObj)
        {
            try
            {
                var filtro = Builders<Lot>.Filter.Eq("_id", ObjectId.Parse(id));

                var update = Builders<Lot>.Update.Combine();

                foreach (var property in typeof(Lot).GetProperties())
                {
                    if (property.GetValue(lotObj) != null && property.Name != "_Id")
                    {
                        update = update.Set(property.Name, property.GetValue(lotObj));
                    }
                }

                object value = await _lotCollection.UpdateOneAsync(filtro, update);

                return (value as Lot)!;
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

        public async Task<object> DeleteMany<T>(List<string> Lot)
        {
            try
            {
                var filtro = Builders<Lot>.Filter.In("_id", Lot);

                var deleteResult = await _lotCollection.DeleteManyAsync(filtro);
                if (deleteResult.DeletedCount == 1)
                    return "Lote deletado";
                else
                    throw new SaveLotException("algo deu errado ao deletar");
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

        public async Task<Lot> GetLotByIdVariant<T>(string idVariant)
        {
            try
            {
                var filtro = Builders<Lot>.Filter.Eq("IdVariant", ObjectId.Parse(idVariant));

                var pResult = _lotCollection.Find(filtro).ToList();
                
                return pResult.FirstOrDefault() ?? throw new FindByIdEventException("Lote não encontrado");
            }
            catch (FindByIdEventException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<object> Save<T>(object Lot)
        {
            try
            {
                await _lotCollection.InsertOneAsync(Lot as Lot);
                return ((Lot)Lot).Id;
            }
            catch (SaveLotException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<object> SaveMany<T>(List<Lot> Lot)
        {
            try
            {
                await _lotCollection.InsertManyAsync(Lot);
                return Lot;
            }
            catch (SaveLotException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}