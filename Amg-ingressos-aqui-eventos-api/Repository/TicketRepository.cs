using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Repository.Querys;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Consts;


namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TicketRepository<T> : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;
        private readonly IMongoCollection<StatusTicketsRow> _ticketStatusCollection;
        private readonly MongoClient _mongoClient;

        public TicketRepository(
            IDbConnection<Ticket> dbConnection,
            IDbConnection<StatusTicketsRow> dbConnectionCourtesy
        )
        {
            _mongoClient = dbConnection.GetClient();
            _ticketCollection = dbConnection.GetConnection("tickets");
            _ticketStatusCollection = dbConnectionCourtesy.GetConnection("statusCourtesyTickets");
        }

        public async Task<List<GetTicketDataEvent>> GetByUser<T1>(string idUser)
        {
            try
            {
                BsonDocument documentFilter = BsonDocument.Parse(
                    @"{$addFields:{'IdUser': { '$toString': '$IdUser' }}}"
                );
                BsonDocument documentFilter1 = BsonDocument.Parse(
                    @"{ $match: { '$and': [{ 'IdUser': '" + idUser.ToString() + "' }] }}"
                );

                BsonDocument lookupLots = BsonDocument.Parse(
                    @"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'lots',
                                                                            localField: 'IdLot',
                                                                            foreignField: '_id',
                                                                            as: 'Lot'
                                                                        }
                                                                }"
                );
                BsonDocument lookupVariants = BsonDocument.Parse(
                    @"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'variants',
                                                                            localField: 'Lot.IdVariant',
                                                                            foreignField: '_id',
                                                                            as: 'Variant'
                                                                        }
                                                                }"
                );
                BsonDocument lookupEvents = BsonDocument.Parse(
                    @"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'events',
                                                                            localField: 'Variant.IdEvent',
                                                                            foreignField: '_id',
                                                                            as: 'Event'
                                                                        }
                                                                }"
                );
                BsonDocument uniwindLot = BsonDocument.Parse(
                    @"{
                                                                    $unwind: '$Lot'
                                                                }"
                );
                BsonDocument uniwindVariant = BsonDocument.Parse(
                    @"{
                                                                    $unwind: '$Variant'
                                                                }"
                );
                BsonDocument uniwindEvent = BsonDocument.Parse(
                    @"{
                                                                    $unwind: '$Event'
                                                                }"
                );

                BsonDocument[] pipeline = new BsonDocument[]
                {
                    documentFilter,
                    documentFilter1,
                    lookupLots,
                    lookupVariants,
                    lookupEvents,
                    uniwindLot,
                    uniwindVariant,
                    uniwindEvent
                };

                // Execute a agregação na coleção de ingressos
                var result = await _ticketCollection
                    .AggregateAsync<GetTicketDataEvent>(pipeline)
                    .Result
                    .ToListAsync();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> SaveAsync<T1>(object ticket)
        {
            try
            {
                var data = ticket as Ticket ??
                    throw new SaveException(string.Format(MessageLogErrors.Save,this.GetType().Name,"ticket"));

                await _ticketCollection.InsertOneAsync(data);
                return data.Id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> DeleteMany<T1>(List<string> tickets)
        {
            try
            {
                var filtro = Builders<Ticket>.Filter.In("_id", tickets);
                var result = await _ticketCollection.DeleteManyAsync(filtro);

                if (result.DeletedCount >= 1)
                    return "Ingressos Deletado";
                else
                    throw new DeleteException(string.Format(MessageLogErrors.Delete,this.GetType().Name,"Ticket"));
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> DeleteByLot<T1>(string idLot)
        {
            try
            {
                var filtro = Builders<Ticket>.Filter.Eq("IdLot", idLot);
                var result = await _ticketCollection.DeleteManyAsync(filtro);

                if (result.DeletedCount >= 1)
                    return "Ingressos Deletado";
                else
                    throw new DeleteException(string.Format(MessageLogErrors.Delete,this.GetType().Name,"Ticket"));
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetTickets<T1>(Ticket ticket)
        {
            try
            {
                var builder = Builders<Ticket>.Filter;
                var filter = builder.Empty;
                filter &= builder.Eq(x => x.IdUser, null);

                if (!string.IsNullOrWhiteSpace(ticket.Id))
                    filter &= builder.Eq(x => x.Id, ticket.Id);

                if (!string.IsNullOrEmpty(ticket.IdLot))
                    filter &= builder.Eq(x => x.IdLot, ticket.IdLot);

                if (!string.IsNullOrEmpty(ticket.IdUser))
                    filter &= builder.Eq(x => x.IdUser, null);

                var result = await _ticketCollection.FindAsync(filter).Result.ToListAsync()
                ?? throw new GetException("Ticket não encontrado");

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<string>> GetTicketsByLot<T1>(string idLot)
        {
            try
            {
                var filter = Builders<Ticket>.Filter.Eq("IdLot", idLot);
                var tickets = await _ticketCollection.Find(filter).ToListAsync();

                var result =
                    tickets.Select(e => e.Id).ToList()
                    ?? throw new GetException("Ticket não encontrado");

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Ticket> GetById<T1>(string idTicket)
        {
            try
            {
                var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(idTicket));
                var tickets = await _ticketCollection.Find(filter).FirstOrDefaultAsync();

                var result =
                    tickets ?? throw new GetException("Ticket não encontrado");

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> EditAsync<T1>(string id, Ticket ticket)
        {
            try
            {
                var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Ticket>.Update
                    .Set("IdUser", ticket.IdUser)
                    .Set("Value", ticket.Value)
                    .Set("isSold", ticket.IsSold)
                    .Set("Position", ticket.Position)
                    .Set("QrCode", ticket.QrCode)
                    .Set("Status", ticket.Status)
                    .Set("IdColab", ticket.IdColab);

                // Busca os tickets que correspondem ao filtro
                var result = await _ticketCollection.UpdateOneAsync(filter, update);

                return await _ticketCollection.Find(filter).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> GetByIdWithDataUser<T1>(string id)
        {
            try
            {
                var json = QuerysMongo.GetTicketByIdDataUser;
                var json1 = QuerysMongo.GetTicketByIdDataLot;
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument document1 = BsonDocument.Parse(json1);

                BsonDocument documentFilter = BsonDocument.Parse(
                    @"{$addFields:{'_id': { '$toString': '$_id' }}}"
                );
                BsonDocument documentFilter1 = BsonDocument.Parse(
                    @"{ $match: { '$and': [{ '_id': '" + id.ToString() + "' }] }}"
                );
                BsonDocument[] pipeline = new BsonDocument[]
                {
                    documentFilter,
                    documentFilter1,
                    document,
                    document1,
                };
                GetTicketDataUser pResults =
                    await _ticketCollection
                        .AggregateAsync<GetTicketDataUser>(pipeline)
                        .Result
                        .FirstOrDefaultAsync()
                    ?? throw new GetException("Evento não encontrado");
                return pResults;
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> GetByIdWithDataEvent<T1>(string id)
        {
            try
            {
                BsonDocument documentFilter = BsonDocument.Parse(
                    @"{$addFields:{'_id': { '$toString': '$_id' }}}"
                );
                BsonDocument documentFilter1 = BsonDocument.Parse(
                    @"{ $match: { '$and': [{ '_id': '" + id.ToString() + "' }] }}"
                );
                BsonDocument lookupLots = BsonDocument.Parse(
                    @"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'lots',
                                                                            localField: 'IdLot',
                                                                            foreignField: '_id',
                                                                            as: 'Lot'
                                                                        }
                                                                }"
                );
                BsonDocument lookupVariants = BsonDocument.Parse(
                    @"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'variants',
                                                                            localField: 'Lot.IdVariant',
                                                                            foreignField: '_id',
                                                                            as: 'Variant'
                                                                        }
                                                                }"
                );
                BsonDocument lookupEvents = BsonDocument.Parse(
                    @"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'events',
                                                                            localField: 'Variant.IdEvent',
                                                                            foreignField: '_id',
                                                                            as: 'Event'
                                                                        }
                                                                }"
                );
                BsonDocument uniwindLot = BsonDocument.Parse(
                    @"{
                                                                    $unwind: '$Lot'
                                                                }"
                );
                BsonDocument uniwindVariant = BsonDocument.Parse(
                    @"{
                                                                    $unwind: '$Variant'
                                                                }"
                );
                BsonDocument uniwindEvent = BsonDocument.Parse(
                    @"{
                                                                    $unwind: '$Event'
                                                                }"
                );
                BsonDocument[] pipeline = new BsonDocument[]
                {
                    documentFilter,
                    documentFilter1,
                    lookupLots,
                    lookupVariants,
                    lookupEvents,
                    uniwindLot,
                    uniwindVariant,
                    uniwindEvent
                };
                GetTicketDataEvent pResults =
                    await _ticketCollection
                        .AggregateAsync<GetTicketDataEvent>(pipeline)
                        .Result
                        .FirstOrDefaultAsync()
                    ?? throw new GetException("Ticket não encontrado");
                return pResults;
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> SaveMany(List<Ticket> lstTicket)
        {
            try
            {
                await _ticketCollection.InsertManyAsync(lstTicket);
                return "Ok";
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> BurnTicketsAsync<T1>(string id, int status)
        {
            try
            {
                //Where do update
                var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));

                //Set do update
                var update = Builders<Ticket>.Update.Set("Status", status);

                //Executa o comando
                await _ticketCollection.UpdateOneAsync(filter, update);
                return new object();
            }
            catch
            {
                throw;
            }
        }
    }
}