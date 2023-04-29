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
                var result = await _eventCollection.FindAsync<Event>(x => x._Id == id as string)
                    .Result.FirstOrDefaultAsync();
                

                if (result == null)
                    throw new FindByIdEventException("Evento não encontrado");

                return result;
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

        public async Task<IEnumerable<object>> GetAllEvents<T>()
        {
            try
            {
                var json = @"{
                                $lookup: {
                                    from: 'variants',
                                    'let': { eventId : { '$toString': '$_id' }},
                                    pipeline: [
                                        {
                                            $match: {
                                                $expr: {
                                                    $eq: [
                                                        '$IdEvent',
                                                        '$$eventId'
                                                    ]
                                                }
                                            }
                                        },
                                        {
                                            $lookup: {
                                                from: 'lots',
                                                'let': { variantId : { '$toString': '$_id' }},
                                                pipeline: [
                                                    {
                                                        $match: {
                                                            $expr: {
                                                                $eq: [
                                                                    '$IdVariant',
                                                                    '$$variantId'
                                                                ]
                                                            }
                                                        }
                                                    }
                                                ],
                                                as: 'Lot'
                                            }
                                        },
                                    ],
                                    as: 'Variant'
                                }
                            }";
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[] { 
                    document
                };
                List<GetEvents> pResults = _eventCollection
                                                .Aggregate<GetEvents>(pipeline).ToList();
                //var result = await _eventCollection.Find(_ => true).ToListAsync();
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

        public async Task<object> Save<T>(object eventComplet)
        {
            try
            {
                await _eventCollection.InsertOneAsync(eventComplet as Event);
                return (eventComplet as Event)._Id;
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