using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Repository.Querys;
using Amg_ingressos_aqui_eventos_api.Model.Querys;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TicketRepository<T> : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;
        private readonly MongoClient _mongoClient;
        public TicketRepository(IDbConnection<Ticket> dbConnection)
        {
            _ticketCollection = dbConnection.GetConnection("tickets");
            _mongoClient = dbConnection.GetClient();
        }

        public async Task<object> Save<T>(object ticket)
        {
            try
            {
                await _ticketCollection.InsertOneAsync(ticket as Ticket);
                return ((Ticket)ticket).Id;
            }
            catch (SaveTicketException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> DeleteMany<T>(List<string> tickets)
        {

            try
            {
                var filtro = Builders<Ticket>.Filter.In("_id", tickets);
                var result = await _ticketCollection.DeleteManyAsync(filtro);

                if (result.DeletedCount >= 1)
                    return "Ingressos Deletado";
                else
                    throw new DeleteEventException("Evento não encontrado");
            }
            catch (SaveTicketException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> DeleteByLot<T>(string idLot)
        {

            try
            {
                var filtro = Builders<Ticket>.Filter.Eq("IdLot", idLot);

                var result = await _ticketCollection.DeleteManyAsync(filtro);

                if (result.DeletedCount >= 1)
                    return "Ingressos Deletado";
                else
                    throw new DeleteEventException("Ingressos não encontrados");
            }
            catch (SaveTicketException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Ticket>> GetTickets<T>(Ticket ticket)
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

                var result = await _ticketCollection.Find(filter).ToListAsync();

                if (result == null)
                    throw new FindTicketByUserException("Ticket não encontrado");

                return result;
            }
            catch (FindTicketByUserException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<string>> GetTicketsByLot<T>(string idLot)
        {
            try
            {
                var filter = Builders<Ticket>.Filter.Eq("IdLot", idLot);

                var tickets = await _ticketCollection.Find(filter).ToListAsync();

                var result = tickets.Select(e => e.Id).ToList() ?? throw new FindTicketByUserException("Ticket não encontrado");

                return result;
            }
            catch (FindTicketByUserException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> UpdateTicketsAsync<T>(string id, Ticket ticket)
        {
            try
            {
                // Cria um filtro para buscar tickets com o ID do lot especificado
                var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Ticket>.Update
                        .Set("IdUser", ticket.IdUser)
                        .Set("Value", ticket.Value)
                        .Set("isSold", ticket.isSold)
                        .Set("Position", ticket.Position)
                        .Set("QrCode", ticket.QrCode);

                // Busca os tickets que correspondem ao filtro
                var result = await _ticketCollection.UpdateOneAsync(filter, update);

                if (result.ModifiedCount == 0)
                {
                    throw new NotModificateTicketsExeption("O ticket não foi atualizado");
                }

                return await _ticketCollection.Find(filter).ToListAsync();
            }
            catch (NotModificateTicketsExeption ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> GetTicketByIdDataUser<T>(string id)
        {
            try
            {
                var json = QuerysMongo.GetTicketByIdDataUser;
                var json1 = QuerysMongo.GetTicketByIdDataLot;
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument document1 = BsonDocument.Parse(json1);

                BsonDocument documentFilter = BsonDocument.Parse(@"{$addFields:{'_id': { '$toString': '$_id' }}}");
                BsonDocument documentFilter1 = BsonDocument.Parse(@"{ $match: { '$and': [{ '_id': '" + id.ToString() + "' }] }}");
                BsonDocument[] pipeline = new BsonDocument[] {
                    documentFilter,
                    documentFilter1,
                    document,
                    document1,
                };
                GetTicketDataUser pResults = await _ticketCollection
                                                .Aggregate<GetTicketDataUser>(pipeline)
                                                .FirstOrDefaultAsync() ?? throw new FindByIdEventException("Evento não encontrado");
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
        public async Task<object> GetTicketByIdDataEvent<T>(string id)
        {
            try
            {
                //var json = QuerysMongo.GetTicketByIdDataEvent;
                //BsonDocument document = BsonDocument.Parse(@json);

                BsonDocument documentFilter = BsonDocument.Parse(@"{$addFields:{'_id': { '$toString': '$_id' }}}");
                BsonDocument documentFilter1 = BsonDocument.Parse(@"{ $match: { '$and': [{ '_id': '" + id.ToString() + "' }] }}");
                BsonDocument lookupLots = BsonDocument.Parse(@"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'lots',
                                                                            localField: 'IdLot',
                                                                            foreignField: '_id',
                                                                            as: 'Lot'
                                                                        }
                                                                }");
                BsonDocument lookupVariants = BsonDocument.Parse(@"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'variants',
                                                                            localField: 'Lot.IdVariant',
                                                                            foreignField: '_id',
                                                                            as: 'Variant'
                                                                        }
                                                                }");
                BsonDocument lookupEvents = BsonDocument.Parse(@"{
                                                                    $lookup:
                                                                        {
                                                                            from: 'events',
                                                                            localField: 'Variant.IdEvent',
                                                                            foreignField: '_id',
                                                                            as: 'Event'
                                                                        }
                                                                }");
                BsonDocument uniwindLot = BsonDocument.Parse(@"{
                                                                    $unwind: '$Lot'
                                                                }");
                BsonDocument uniwindVariant = BsonDocument.Parse(@"{
                                                                    $unwind: '$Variant'
                                                                }");
                BsonDocument uniwindEvent = BsonDocument.Parse(@"{
                                                                    $unwind: '$Event'
                                                                }");
                BsonDocument[] pipeline = new BsonDocument[] {
                    documentFilter,
                    documentFilter1,
                    lookupLots,
                    lookupVariants,
                    lookupEvents,
                    uniwindLot,
                    uniwindVariant,
                    uniwindEvent
                };
                GetTicketDataEvent pResults = await _ticketCollection
                                                .Aggregate<GetTicketDataEvent>(pipeline)
                                                .FirstOrDefaultAsync() ?? throw new FindByIdEventException("Evento não encontrado");
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
        public async Task<object> SaveMany(List<Ticket> lstTicket)
        {
            try
            {
                _ticketCollection.InsertMany(lstTicket);
            }
            catch (SaveTicketException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Ok";
        }
    }
}