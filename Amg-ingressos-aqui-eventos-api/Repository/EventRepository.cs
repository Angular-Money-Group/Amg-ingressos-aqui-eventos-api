using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using MongoDB.Driver.Core.Operations;
using MongoDB.Bson.Serialization;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Repository.Querys;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class EventRepository<T> : IEventRepository
    {
        private readonly IMongoCollection<Event> _eventCollection;
        public EventRepository(IDbConnection<Event> dbconnection)
        {
            _eventCollection = dbconnection.GetConnection("events");
        }
        public async Task<object> Delete<T>(object id)
        {
            try
            {
                var result = await _eventCollection.DeleteOneAsync(x => x._Id == id as string);
                if (result.DeletedCount >= 1)
                    return "Evento Deletado";
                else
                    throw new DeleteEventException("Evento não encontrado");
            }
            catch (DeleteEventException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> FindById<T>(object id)
        {
            try
            {
                var json = QuerysMongo.GetEventQuery;

                BsonDocument documentFilter = BsonDocument.Parse(@"{$addFields:{'_id': { '$toString': '$_id' }}}");
                BsonDocument documentFilter1 = BsonDocument.Parse(@"{ $match: { '$and': [{ '_id': '" + id.ToString() + "' }] }}");
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[] {
                    documentFilter,
                    documentFilter1,
                    document
                };
                List<GetEvents> pResults = _eventCollection
                                                .Aggregate<GetEvents>(pipeline).ToList();

                //var result = await _eventCollection.FindAsync<Event>(x => x._Id == id as string)
                //    .Result.FirstOrDefaultAsync();


                if (pResults == null)
                    throw new FindByIdEventException("Evento não encontrado");

                return pResults;
            }
            catch (FindByIdEventException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Event>> GetAllEvents<T>()
        {
            try
            {
                List<Event> pResults = _eventCollection.Find(Builders<Event>.Filter.Empty).ToList();
                if (!pResults.Any())
                    throw new GetAllEventException("Eventos não encontrados");

                return pResults;
            }
            catch (GetAllEventException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public Task<List<Event>> GetWeeklyEvents<T>(IPagination paginationOptions)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
                DateTime startOfRange = startOfWeek.AddDays(-1); // domingo
                DateTime endOfRange = startOfWeek.AddDays(6); // sábado

                var filter = Builders<Event>.Filter.And(
                    Builders<Event>.Filter.Gte(e => e.StartDate, startOfRange),
                    Builders<Event>.Filter.Lt(e => e.StartDate, endOfRange.AddDays(1))
                );

                List<Event> pResults = _eventCollection.Find(filter).ToList()
                .Skip((paginationOptions.page - 1) * paginationOptions.pageSize)
                .Take(paginationOptions.pageSize)
                .ToList();
                if (!pResults.Any())
                    throw new GetAllEventException("Eventos não encontrados");

                return Task.FromResult(pResults);
            }
            catch (GetAllEventException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Save<T>(object eventComplet)
        {
            try
            {
                await _eventCollection.InsertOneAsync(eventComplet as Event);
                return (eventComplet as Event)!._Id!;
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
    }
}