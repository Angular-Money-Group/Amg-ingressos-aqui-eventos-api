using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Repository.Querys;
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

        public async Task<object> Delete<T1>(object id)
        {
            var filter = Builders<Event>.Filter.Eq("_Id", id);

            var update = Builders<Event>.Update.Set("Status", Enum.EnumStatusEvent.Canceled);

            var action = await _eventCollection.UpdateOneAsync(filter, update);

            if (action.MatchedCount == 0)
            {
                throw new DeleteException("Evento não encontrado");
            }
            else if (action.ModifiedCount == 0)
            {
                throw new DeleteException("Evento já excluido");
            }

            return "Evento deletado com sucesso";
        }

        public async Task<T1> GetById<T1>(string id)
        {
            /*var json = QuerysMongo.GetEventQuery;

            BsonDocument documentFilter1 = BsonDocument.Parse(
                @"{ $match: { '$and': [{ '_id': ObjectId('" + id.ToString() + "') }] }}"
            );
            BsonDocument document = BsonDocument.Parse(json);
            BsonDocument[] pipeline = new BsonDocument[]
            {
                    documentFilter1,
                    document
            };*/
            var eventData = await _eventCollection.Aggregate()
                     .Match(new BsonDocument { { "_id", ObjectId.Parse(id) } })
                     .Lookup("variants", "_id", "IdEvent", "Variants")
                     .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                     .As<T1>()
                     .ToListAsync();

            /*List<GetEvents> pResults =
                (List<GetEvents>)(await _eventCollection.AggregateAsync<GetEvents>(pipeline)
                ?? throw new GetException("Evento não encontrado"));*/
            return eventData[0];
        }

        public async Task<Event> GetByIdVariant<T1>(string id)
        {
            var filter = Builders<Event>.Filter.Eq("_Id", ObjectId.Parse(id));

            var pResult = await _eventCollection.FindAsync(filter);

            return pResult.FirstOrDefault()
                ?? throw new GetException("Evento não encontrado");
        }

        public async Task<List<Event>> GetByName<T1>(string name)
        {
            var filter = Builders<Event>.Filter.Regex(
                g => g.Name,
                new BsonRegularExpression(name, "i")
            );

            List<Event> pResults = await _eventCollection.Find(filter).ToListAsync();
            if (!pResults.Any())
                throw new GetException("Eventos não encontrados");

            return pResults;
        }

        public async Task<Event> SetHighlightEvent<T1>(string id)
        {
            List<Event> pHighlightedEvents = _eventCollection
                .Find(Builders<Event>.Filter.Eq("Highlighted", true))
                .ToList();

            if (pHighlightedEvents.Count >= 9)
                throw new GetException("Maximo de Eventos destacados atingido");

            var filter = Builders<Event>.Filter.Eq("_Id", id);

            Event eventDoc = (Event)await _eventCollection.FindAsync(filter);

            if (eventDoc == null)
                throw new GetException("Evento não encontrado");

            var isHighlighted = eventDoc.Highlighted;
            var newValue = !isHighlighted;

            var updated = Builders<Event>.Update.Set(e => e.Highlighted, newValue);

            _eventCollection.UpdateOne(filter, updated);

            eventDoc.Highlighted = !eventDoc.Highlighted;
            return eventDoc;
        }

        public async Task<List<GetEventsWithNames>> GetAllEvents<T1>(Pagination paginationOptions)
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

            List<GetEventsWithNames> pResults = (List<GetEventsWithNames>)await _eventCollection
                .AggregateAsync<GetEventsWithNames>(pipeline);

            if (!pResults.Any())
                throw new GetException("Eventos não encontrados");

            pResults.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));

            int startIndex = (paginationOptions.Page - 1) * paginationOptions.PageSize;
            List<GetEventsWithNames> pagedResults = pResults
                .Skip(startIndex)
                .Take(paginationOptions.PageSize)
                .ToList();

            return pagedResults;
        }

        public async Task<List<GetEventsWithNames>> GetWithUserData<T1>()
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
                    BsonDocument.Parse("{ $sort: { 'Highlighted': -1, 'Status': 1, 'StartDate': 1 } }")
                };

            List<GetEventsWithNames> pResults = await _eventCollection
                .AggregateAsync<GetEventsWithNames>(pipeline).Result.ToListAsync();

            if (!pResults.Any())
                throw new GetException("Eventos não encontrados");

            List<GetEventsWithNames> pagedResults = pResults.ToList();
            return pagedResults;
        }

        public async Task<List<Event>> GetWeeklyEvents<T1>(Pagination paginationOptions)
        {
            DateTime now = DateTime.Now;
            DateTime startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
            DateTime startOfRange = startOfWeek.AddDays(-1); // domingo
            DateTime endOfRange = startOfWeek.AddDays(6); // sábado

            var filter = Builders<Event>.Filter.And(
                Builders<Event>.Filter.Gte(e => e.StartDate, startOfRange),
                Builders<Event>.Filter.Lt(e => e.StartDate, endOfRange.AddDays(1)),
                Builders<Event>.Filter.Eq("Status", Enum.EnumStatusEvent.Active)
            );

            List<Event> pResults = await _eventCollection
                .FindAsync(filter).Result.ToListAsync();
            var listResult =
                pResults
                .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .ToList();

            if (!listResult.Any())
                throw new GetException("Eventos não encontrados");

            return listResult;
        }

        public async Task<List<Event>> GetHighlightedEvents<T1>(Pagination paginationOptions)
        {
            var filter = Builders<Event>.Filter.And(
                Builders<Event>.Filter.Eq("Highlighted", true),
                Builders<Event>.Filter.Eq("Status", Enum.EnumStatusEvent.Active)
            );

            List<Event> pResults = await _eventCollection
                .FindAsync(filter).Result.ToListAsync();

            var listResult =
            pResults
            .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
            .Take(paginationOptions.PageSize)
            .ToList();
            if (!listResult.Any())
                throw new GetException("Eventos não encontrados");

            return listResult;
        }

        public async Task<List<Event>> GetByProducer<T1>(
            string id,
            Pagination paginationOptions,
            FilterOptions? filterOptions
        )
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

            List<Event> pResults = await _eventCollection
                .FindAsync(filter).Result
                .ToListAsync();

            if (pResults.Count == 0)
            {
                throw new GetException("Eventos não encontrados");
            }

            return pResults;
        }

        public async Task<object> Save<T1>(object eventComplet)
        {
            var data = eventComplet as Event ??
                throw new SaveException("Algo deu errado ao salvar evento");
            await _eventCollection.InsertOneAsync(data);
            return data.Id;
        }

        public async Task<Event> Edit<T1>(string id, Event eventObj)
        {
            var filtro = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Event>.Update.Combine();

            foreach (var property in typeof(Event).GetProperties())
            {
                if (property.GetValue(eventObj) != null && property.Name != "_Id" && property.Name != "Variant")
                {
                    update = update.Set(property.Name, property.GetValue(eventObj));
                }
            }

            await _eventCollection.UpdateOneAsync(filtro, update);

            return eventObj;
        }

        public async Task<List<Model.Querys.GetEventwithTicket.GetEventWithTickets>> GetAllEventsWithTickets(
            string idEvent,
            string idOrganizer
        )
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

            List<Model.Querys.GetEventwithTicket.GetEventWithTickets> pResults = await _eventCollection
                .AggregateAsync<Model.Querys.GetEventwithTicket.GetEventWithTickets>(pipeline)
                .Result
                .ToListAsync();

            if (!pResults.Any())
                throw new GetException("Evento não encontrado");

            return pResults;
        }

        public async Task<List<GetEventTransactions>> GetAllEventsWithTransactions(
            string idEvent,
            string idOrganizer
        )
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

            List<GetEventTransactions> pResults = await _eventCollection
                .AggregateAsync<GetEventTransactions>(pipeline)
                .Result
                .ToListAsync();

            if (!pResults.Any())
                throw new GetException("Evento não encontrado");

            return pResults;
        }
    }
}