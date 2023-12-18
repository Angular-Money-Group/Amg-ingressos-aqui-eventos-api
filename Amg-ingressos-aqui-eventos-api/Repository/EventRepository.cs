using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Enum;

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
            var eventData = await _eventCollection.Aggregate()
                     .Match(new BsonDocument { { "_id", ObjectId.Parse(id) } })
                     .Lookup("variants", "_id", "IdEvent", "Variants")
                     .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                     .As<T1>()
                     .ToListAsync();

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

        public async Task<List<T1>> GetAllEvents<T1>(Pagination paginationOptions, Event? eventModel)
        {
            var eventData = await _eventCollection.Aggregate()
                    .Match(GenerateFilterGetEvents(eventModel))
                    .Lookup("variants", "_id", "IdEvent", "Variants")
                    .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                    .Lookup("user", "IdOrganizer", "_id", "User")
                    .Sort(new BsonDocument ("StartDate", 1))
                    .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                    .Limit(paginationOptions.PageSize)
                    .As<T1>()
                    .ToListAsync();

            return eventData;
        }
        public FilterDefinition<Event> GenerateFilterGetEvents(Event? eventModel)
        {
            FilterDefinition<Event> arrayFilter =
                Builders<Event>.Filter.Eq("Highlighted", eventModel?.Highlighted ?? false);
            
            if (eventModel != null)
            {
                arrayFilter = arrayFilter & Builders<Event>.Filter.Eq("Status", eventModel.Status);
                if (!string.IsNullOrEmpty(eventModel.Id))
                    arrayFilter = arrayFilter & Builders<Event>.Filter.Eq("_Id", eventModel.Id);
                else if (eventModel.StartDate != DateTime.MinValue)
                    arrayFilter = arrayFilter & Builders<Event>.Filter.Gte("StartDate", eventModel.StartDate);
            }
            return arrayFilter;
        }

        public async Task<List<T1>> GetWithUserData<T1>()
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

            List<T1> pResults = await _eventCollection
                .AggregateAsync<T1>(pipeline).Result.ToListAsync();

            if (!pResults.Any())
                throw new GetException("Eventos não encontrados");

            List<T1> pagedResults = pResults.ToList();
            return pagedResults;
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

        public async Task<List<T1>> GetAllEventsWithTickets<T1>(string idEvent, string idOrganizer)
        {
            BsonDocument documentFilter = !string.IsNullOrEmpty(idEvent)
                ? new BsonDocument { { "_id", ObjectId.Parse(idEvent) } }
                : new BsonDocument { { "IdOrganizer", ObjectId.Parse(idOrganizer) } };

            var eventData = await _eventCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("variants", "_id", "IdEvent", "Variants")
                    .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                    .Lookup("tickets", "Lots._id", "IdLot", "Tickets")
                    .Lookup("user", "IdOrganizer", "_id", "User")
                    .As<T1>()
                    .ToListAsync();

            return eventData;
        }

        public async Task<List<T1>> GetAllEventsWithTransactions<T1>(
            string idEvent,
            string idOrganizer
        )
        {
            BsonDocument documentFilter = !string.IsNullOrEmpty(idEvent)
                ? new BsonDocument { { "_id", ObjectId.Parse(idEvent) } }
                : new BsonDocument { { "IdOrganizer", ObjectId.Parse(idOrganizer) } };

            var eventData = await _eventCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("variants", "_id", "IdEvent", "Variants")
                    .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                    .Lookup("tickets", "Lots._id", "IdLot", "Tickets")
                    .Lookup("user", "IdOrganizer", "_id", "User")
                    .Lookup("transaction", "_id", "IdEvent", "Transactions")
                    .As<T1>()
                    .ToListAsync();

            return eventData;
        }
    }
}