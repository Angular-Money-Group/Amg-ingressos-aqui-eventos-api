using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Repository.Querys;
using Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket;
using Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventTransactions;

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
                var filter = Builders<Event>.Filter.Eq("_Id", id);

                var update = Builders<Event>.Update.Set("Status", Enum.StatusEvent.Canceled);

                var action = await _eventCollection.UpdateOneAsync(filter, update);

                if (action.MatchedCount == 0)
                {
                    throw new DeleteEventException("Evento não encontrado");
                }
                else if (action.ModifiedCount == 0)
                {
                    throw new DeleteEventException("Evento já excluido");
                }

                var value = "Evento deletado com sucesso";
                return value;
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

                BsonDocument documentFilter1 = BsonDocument.Parse(
                    @"{ $match: { '$and': [{ '_id': ObjectId('" + id.ToString() + "') }] }}"
                );
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[]
                {
                    //documentFilter,
                    documentFilter1,
                    document
                };
                List<GetEvents> pResults =
                    _eventCollection.Aggregate<GetEvents>(pipeline).ToList()
                    ?? throw new FindByIdEventException("Evento não encontrado");
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

        public async Task<Event> FindByIdVariant<T>(string id)
        {
            try
            {
                var filter = Builders<Event>.Filter.Eq("_Id", ObjectId.Parse(id));

                var pResult = _eventCollection.Find(filter).ToList();

                return pResult.FirstOrDefault()
                    ?? throw new FindByIdEventException("Evento não encontrado");
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

        public async Task<List<Event>> FindByName<T>(string nome)
        {
            try
            {
                var filter = Builders<Event>.Filter.Regex(
                    g => g.Name,
                    new BsonRegularExpression(nome, "i")
                );

                List<Event> pResults = await _eventCollection.Find(filter).ToListAsync();
                if (!pResults.Any())
                    throw new FindByDescriptionException("Eventos não encontrados");

                return pResults;
            }
            catch (FindByDescriptionException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Event> SetHighlightEvent<T>(string id)
        {
            try
            {
                List<Event> pHighlightedEvents = _eventCollection
                    .Find(Builders<Event>.Filter.Eq("Highlighted", true))
                    .ToList();

                if (pHighlightedEvents.Count >= 9)
                    throw new MaxHighlightedEvents("Maximo de Eventos destacados atingido");

                var filter = Builders<Event>.Filter.Eq("_Id", id);

                Event eventDoc = _eventCollection.Find(filter).FirstOrDefault();

                if (eventDoc == null)
                    throw new FindByDescriptionException("Evento não encontrado");

                var isHighlighted = eventDoc.Highlighted;
                var newValue = !isHighlighted;

                var updated = Builders<Event>.Update.Set(e => e.Highlighted, newValue);

                var pResults = _eventCollection.UpdateOne(filter, updated);

                eventDoc.Highlighted = !eventDoc.Highlighted;
                return eventDoc;
            }
            catch (FindByDescriptionException ex)
            {
                throw ex;
            }
            catch (MaxHighlightedEvents ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetEventsWithNames>> GetAllEvents<T>(Pagination paginationOptions)
        {
            try
            {
                List<BsonDocument> pipeline = new List<BsonDocument>
                {
                    BsonDocument.Parse(
                        @"
                    {
                        $lookup: {
                            from: 'user',
                            'let': { idOrganizer : { '$toString': '$IdOrganizer' }},
                            pipeline: [
                                {
                                    $match: {
                                        $expr: {
                                            $eq: [{ '$toString': '$_id' },'$$idOrganizer']
                                        }
                                    }
                                },
                                                      {
                            $project: {
                                'Name': 1,
                                _id: 0
                            }
                        }
                            ],
                            as: 'User'
                        }
                    }"
                    ),
                    BsonDocument.Parse("{ $unwind: '$User' }"),
                    BsonDocument.Parse("{ $match: { 'Status': 0 } }")
                };

                List<GetEventsWithNames> pResults = _eventCollection
                    .Aggregate<GetEventsWithNames>(pipeline)
                    .ToList();

                if (!pResults.Any())
                    throw new GetAllEventException("Eventos não encontrados");

                pResults.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));

                int startIndex = (paginationOptions.page - 1) * paginationOptions.pageSize;
                List<GetEventsWithNames> pagedResults = pResults
                    .Skip(startIndex)
                    .Take(paginationOptions.pageSize)
                    .ToList();
                return pagedResults;
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

        public async Task<List<GetEventsWithNames>> GetAllEventsAdmin<T>()
        {
            try
            {
                List<BsonDocument> pipeline = new List<BsonDocument>
                {
                    BsonDocument.Parse(
                        @"
                    {
                        $lookup: {
                            from: 'user',
                            'let': { idOrganizer : { '$toString': '$IdOrganizer' }},
                            pipeline: [
                                {
                                    $match: {
                                        $expr: {
                                            $eq: [{ '$toString': '$_id' },'$$idOrganizer']
                                        }
                                    }
                                },
                                                      {
                            $project: {
                                'Name': 1,
                                _id: 0
                            }
                        }
                            ],
                            as: 'User'
                        }
                    }"
                    ),
                    BsonDocument.Parse("{ $unwind: '$User' }"),
                    BsonDocument.Parse("{ $sort: { 'highlighted': -1, 'StartDate': 1 } }")
                };

                List<GetEventsWithNames> pResults = _eventCollection
                    .Aggregate<GetEventsWithNames>(pipeline)
                    .ToList();

                if (!pResults.Any())
                    throw new GetAllEventException("Eventos não encontrados");

                List<GetEventsWithNames> pagedResults = pResults.ToList();
                return pagedResults;
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

        public Task<List<Event>> GetWeeklyEvents<T>(Pagination paginationOptions)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
                DateTime startOfRange = startOfWeek.AddDays(-1); // domingo
                DateTime endOfRange = startOfWeek.AddDays(6); // sábado

                var filter = Builders<Event>.Filter.And(
                    Builders<Event>.Filter.Gte(e => e.StartDate, startOfRange),
                    Builders<Event>.Filter.Lt(e => e.StartDate, endOfRange.AddDays(1)),
                    Builders<Event>.Filter.Eq("Status", Enum.StatusEvent.Active)
                );

                List<Event> pResults = _eventCollection
                    .Find(filter)
                    .ToList()
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

        public Task<List<Event>> GetHighlightedEvents<T>(Pagination paginationOptions)
        {
            try
            {
                var filter = Builders<Event>.Filter.And(
                    Builders<Event>.Filter.Eq("Highlighted", true),
                    Builders<Event>.Filter.Eq("Status", Enum.StatusEvent.Active)
                );

                List<Event> pResults = _eventCollection
                    .Find(filter)
                    .ToList()
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

        public async Task<List<Event>> FindByProducer<T>(
            string id,
            Pagination paginationOptions,
            FilterOptions? filterOptions
        )
        {
            try
            {
                var filters = new List<FilterDefinition<Event>>
                {
                    Builders<Event>.Filter.Eq("IdOrganizer", ObjectId.Parse(id))
                };

                if (filterOptions != null)
                {
                    if (!string.IsNullOrEmpty(filterOptions.Name))
                    {
                        filters.Add(
                            Builders<Event>.Filter.Regex(
                                g => g.Name,
                                new BsonRegularExpression(filterOptions.Name, "i")
                            )
                        );
                    }

                    if (!string.IsNullOrEmpty(filterOptions.Local))
                    {
                        filters.Add(
                            Builders<Event>.Filter.Regex(
                                g => g.Local,
                                new BsonRegularExpression(filterOptions.Local, "i")
                            )
                        );
                    }

                    if (filterOptions.StartDate != null)
                    {
                        filters.Add(
                            Builders<Event>.Filter.Gte(g => g.StartDate, filterOptions.StartDate)
                        );
                    }

                    if (filterOptions.EndDate != null)
                    {
                        filters.Add(
                            Builders<Event>.Filter.Lte(g => g.EndDate, filterOptions.EndDate)
                        );
                    }

                    if (!string.IsNullOrEmpty(filterOptions.Type))
                    {
                        var typeMappings = new Dictionary<string, string>
                        {
                            { "show", "Show" },
                            { "apresentacao", "Apresentação" },
                            { "evento", "Evento" }
                        };

                        if (typeMappings.TryGetValue(filterOptions.Type, out var eventType))
                        {
                            filters.Add(Builders<Event>.Filter.Eq(g => g.Type, eventType));
                        }
                    }
                }
                var filter = Builders<Event>.Filter.And(filters);

                SortDefinition<Event> sort = Builders<Event>.Sort.Descending(e => e.StartDate);

                var pResults = _eventCollection
                    .Find(filter)
                    .Sort(sort)
                    .Skip((paginationOptions.page - 1) * paginationOptions.pageSize)
                    .Limit(paginationOptions.pageSize)
                    .ToList();

                if (pResults.Count == 0)
                {
                    throw new GetAllEventException("Eventos não encontrados");
                }

                return pResults;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Save<T>(object eventComplet)
        {
            try
            {
                eventComplet = eventComplet as Event;
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

        public async Task<Event> Edit<T>(string id, Event eventObj)
        {
            try
            {
                var filtro = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));

                var update = Builders<Event>.Update.Combine();

                foreach (var property in typeof(Event).GetProperties())
                {
                    if (property.GetValue(eventObj) != null && property.Name != "_Id")
                    {
                        update = update.Set(property.Name, property.GetValue(eventObj));
                    }
                }

                object value = await _eventCollection.UpdateOneAsync(filtro, update);

                return (eventObj as Event)!;
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

        public async Task<List<GetEventWitTickets>> GetAllEventsWithTickets(
            string idEvent,
            string idOrganizer
        )
        {
            try
            {
                var json = QuerysMongo.GetEventWithTicketsQuery;
                BsonDocument documentFilter1 = !string.IsNullOrEmpty(idEvent)
                    ? BsonDocument.Parse(
                        @"{ $match: { '$and': [{ '_id': ObjectId('"
                            + idEvent.ToString()
                            + "') }] }}"
                    )
                    : BsonDocument.Parse(
                        @"{ $match: { '$and': [{ 'IdOrganizer': ObjectId('"
                            + idOrganizer.ToString()
                            + "') }] }}"
                    );
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[] { documentFilter1, document };

                List<GetEventWitTickets> pResults = _eventCollection
                    .Aggregate<GetEventWitTickets>(pipeline)
                    .ToList();

                if (!pResults.Any())
                    throw new GetAllEventException("Evento não encontrado");

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

        public async Task<List<GetEventTransactions>> GetAllEventsWithTransactions(
            string idEvent,
            string idOrganizer
        )
        {
            try
            {
                var json = QuerysMongo.GetEventWithTransactionsQuery;
                BsonDocument documentFilter1 = !string.IsNullOrEmpty(idEvent)
                    ? BsonDocument.Parse(
                        @"{ $match: { '$and': [{ '_id': ObjectId('"
                            + idEvent.ToString()
                            + "') }] }}"
                    )
                    : BsonDocument.Parse(
                        @"{ $match: { '$and': [{ 'IdOrganizer': ObjectId('"
                            + idOrganizer.ToString()
                            + "') }] }}"
                    );

                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[] { documentFilter1, document };

                List<GetEventTransactions> pResults = _eventCollection
                    .Aggregate<GetEventTransactions>(pipeline)
                    .ToList();

                if (!pResults.Any())
                    throw new GetAllEventException("Evento não encontrado");

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
    }
}
