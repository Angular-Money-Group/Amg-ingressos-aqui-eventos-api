using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using System.Reflection;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Infra;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class EventRepository : IEventRepository
    {
        private readonly IMongoCollection<Event> _eventCollection;

        public EventRepository(IDbConnection dbconnection)
        {
            _eventCollection = dbconnection.GetConnection<Event>("events");
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));

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

            return true;
        }

        public async Task<T> GetById<T>(string id)
        {
            var eventData = await _eventCollection.Aggregate()
                     .Match(new BsonDocument { { "_id", ObjectId.Parse(id) } })
                     .Lookup("variants", "_id", "IdEvent", "Variants")
                     .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                     .As<T>()
                     .ToListAsync();

            if (eventData.Any())
                return eventData[0];
            else
                throw new RuleException("evento nao encontrado");
        }

        public async Task<bool> SetHighlightEvent(string id)
        {
            List<Event> pHighlightedEvents = _eventCollection
                .Find(Builders<Event>.Filter.Eq("Highlighted", true))
                .ToList();

            if (pHighlightedEvents.Count >= 9)
                throw new GetException("Maximo de Eventos destacados atingido");

            var filter = Builders<Event>.Filter.Eq("_Id", id);

            Event eventDoc = GetById<Event>(id).Result;

            if (eventDoc == null)
                throw new GetException("Evento não encontrado");



            var updated = Builders<Event>.Update.Set("Highlighted", !eventDoc.Highlighted);

            await _eventCollection.UpdateOneAsync(filter, updated);
            return true;
        }

        public async Task<List<T>> GetByFilterComplet<T>(Pagination paginationOptions, Event? eventModel)
        {
            var eventData = await _eventCollection.Aggregate()
                    .Match(GenerateFilterGetEvents(eventModel))
                    .Lookup("variants", "_id", "IdEvent", "Variants")
                    .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                    .Lookup("user", "IdOrganizer", "_id", "User")
                    .Sort(new BsonDocument("StartDate", -1))
                    .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                    .Limit(paginationOptions.PageSize)
                    .As<T>()
                    .ToListAsync();

            return eventData;
        }

        public FilterDefinition<Event> GenerateFilterGetEvents(Event? eventModel)
        {
            if (eventModel == null)
                throw new RuleException("evento não pode ser null");

            var filters = new List<FilterDefinition<Event>>();
            Type t = eventModel.GetType();
            PropertyInfo[] pi = t.GetProperties();

            foreach (PropertyInfo p in pi)
            {
                if (p.Name == "Highlighted" && !Convert.ToBoolean(p.GetValue(eventModel)))
                    filters.Add(Builders<Event>.Filter.Eq("Highlighted", p.GetValue(eventModel)));
                else if (p.Name == "Status" && !string.IsNullOrEmpty(Convert.ToString(p.GetValue(eventModel))))
                    filters.Add(Builders<Event>.Filter.Eq("Status", Convert.ToString(p.GetValue(eventModel))));
                else if (p.Name == "StartDate" && Convert.ToDateTime(p.GetValue(eventModel)) != DateTime.MinValue)
                    filters.Add(Builders<Event>.Filter.Gte(p.Name, Convert.ToDateTime(p.GetValue(eventModel))));
                else if (p.Name == "IdOrganizer" && !string.IsNullOrEmpty(Convert.ToString(p.GetValue(eventModel))))
                    filters.Add(Builders<Event>.Filter.Gte(p.Name, Convert.ToString(p.GetValue(eventModel))));
            }
            return Builders<Event>.Filter.And(filters);
        }

        public async Task<Event> Save(Event eventObject)
        {
            var data = eventObject ??
                throw new SaveException("Algo deu errado ao salvar evento");
            await _eventCollection.InsertOneAsync(data);
            return data;
        }

        public async Task<bool> Edit(string id, Event eventObj)
        {
            var filtro = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Event>.Update.Combine();

            foreach (var property in typeof(Event).GetProperties())
            {
                if (property.GetValue(eventObj) != null && property.Name != "_Id"
                && property.Name != "Variant" && property.Name != "IdOrganizer")
                {
                    update = update.Set(property.Name, property.GetValue(eventObj));
                }
            }

            await _eventCollection.UpdateOneAsync(filtro, update);
            return true;
        }

        public async Task<List<T>> GetFilterWithTickets<T>(string idEvent, string idOrganizer)
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
                    .As<T>()
                    .ToListAsync();

            return eventData;
        }

        public async Task<List<T>> GetFilterWithTransactions<T>(string idEvent, string idOrganizer)
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
                    .As<T>()
                    .ToListAsync();

            return eventData;
        }

        public async Task<Event> GetById(string id)
        {
            var filter = Builders<Event>.Filter.Eq("_Id", ObjectId.Parse(id));
            var pResult = await _eventCollection.Find(filter)
                .As<Event>()
                .FirstOrDefaultAsync();

            return pResult;
        }

        public async Task<List<Event>> GetAll()
        {
            var pResult = await _eventCollection.Find(_ => true)
                .As<Event>()
                .ToListAsync();

            return pResult;
        }

        public async Task<(List<T1>, long count)> GetByFilter<T1>(Dictionary<string, object> filters, Pagination? paginationOptions)
        {
            paginationOptions = paginationOptions ?? new Pagination();
            var filter = GenerateFilter(filters);
            long count;

            List<T1> eventData;
            if (filters.Count <= 0)
            {
                eventData = await _eventCollection.Aggregate()
                    .Lookup("variants", "_id", "IdEvent", "Variants")
                    .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                    .Lookup("user", "IdOrganizer", "_id", "User")
                    .Sort(new BsonDocument("StartDate", 1))
                    .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                    .Limit(paginationOptions.PageSize)
                    .As<T1>()
                    .ToListAsync();
                count = _eventCollection.CountDocuments(new BsonDocument());
            }
            else
            {
                eventData = await _eventCollection.Aggregate()
                    .Match(filter)
                    .Lookup("variants", "_id", "IdEvent", "Variants")
                    .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                    .Lookup("user", "IdOrganizer", "_id", "User")
                    .Sort(new BsonDocument("StartDate", 1))
                    .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                    .Limit(paginationOptions.PageSize)
                    .As<T1>()
                    .ToListAsync();
                count = _eventCollection.CountDocuments(filter);
            }

            return (eventData.Any() ? eventData : new List<T1>(), count);
        }

        public async Task<List<T1>> GetByFilter<T1>(Dictionary<string, object> filters)
        {
            var eventData = await _eventCollection.Aggregate()
                    .Match(GenerateFilter(filters))
                    .Lookup("variants", "_id", "IdEvent", "Variants")
                    .Lookup("lots", "Variants._id", "IdVariant", "Lots")
                    .Lookup("user", "IdOrganizer", "_id", "User")
                    .Sort(new BsonDocument("StartDate", 1))
                    .As<T1>()
                    .ToListAsync();

            return eventData.Any() ? eventData : new List<T1>();
        }

        private FilterDefinition<Event> GenerateFilter(Dictionary<string, object> filters)
        {
            var listFilter = new List<FilterDefinition<Event>>();
            foreach (var item in filters)
            {
                switch (item.Key)
                {
                    case "Name":
                        listFilter.Add(GenerateRegexDocument(item.Key, item.Value.ToString() ?? string.Empty));
                        break;
                    case "IdOrganizer":
                        listFilter.Add(GenerateObjectIdDocument(item.Key, item.Value.ToString() ?? string.Empty));
                        break;
                    case "_id":
                        listFilter.Add(GenerateObjectIdDocument(item.Key, item.Value.ToString() ?? string.Empty));
                        break;
                    case "Local":
                        listFilter.Add(GenerateRegexDocument(item.Key, item.Value.ToString() ?? string.Empty));
                        break;
                    case "StartDate":
                        listFilter.Add(Builders<Event>.Filter.Gte(item.Key, item.Value));
                        break;
                    case "EndDate":
                        listFilter.Add(Builders<Event>.Filter.Lte(item.Key, item.Value));
                        break;
                    default:
                        listFilter.Add(Builders<Event>.Filter.Eq(item.Key, item.Value));
                        break;
                }
            }
            return Builders<Event>.Filter.And(listFilter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters">Dicionar com chave valor de filtro exemplo Name,Teste</param>
        /// <returns></returns>
        public async Task<List<Event>> GetByFilter(Dictionary<string, string> filters)
        {
            var listFilter = new List<FilterDefinition<Event>>();
            foreach (var item in filters)
                listFilter.Add(Builders<Event>.Filter.Eq(item.Key.ToString(), item.Value));


            var builders = Builders<Event>.Filter.And(listFilter);
            var pResult = await _eventCollection.Find(builders)
                .As<Event>()
                .ToListAsync();

            return pResult;
        }

        private FilterDefinition<Event> GenerateRegexDocument(string key, string value)
        {
            return Builders<Event>.Filter.Regex(key, new BsonRegularExpression(value, "i"));
        }

        private FilterDefinition<Event> GenerateObjectIdDocument(string key, string value)
        {
            return Builders<Event>.Filter.Eq(key, ObjectId.Parse(value));
        }
    }
}