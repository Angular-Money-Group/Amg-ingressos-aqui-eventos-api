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
            catch (System.Exception ex)
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

        public async Task<object> Delete<T>(string idLot)
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

                if (!string.IsNullOrWhiteSpace(ticket.Id))
                    filter &= builder.Eq(x => x.Id, ticket.Id);

                if (!string.IsNullOrEmpty(ticket.IdLot))
                    filter &= builder.Eq(x => x.IdLot, ticket.IdLot);

                if (!string.IsNullOrEmpty(ticket.IdUser))
                    filter &= builder.Eq(x => x.IdUser, ticket.IdUser);

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

        public async Task<List<string>> GetTicketsByLot<T>(string id)
        {
            try
            {
                var filter = Builders<Ticket>.Filter.Eq("IdLot", id);

                var tickets = await _ticketCollection.Find(filter).ToListAsync();

                var result = tickets.Select(e => e.Id).ToList();

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
        public async Task<object> UpdateTicketsAsync<T>(string id, Ticket ticketObject)
        {
            try
            {
                // Cria um filtro para buscar tickets com o ID do lot especificado
                var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Ticket>.Update
                        .Set("IdUser", ticketObject.IdUser)
                        .Set("Value", ticketObject.Value)
                        .Set("isSold", ticketObject.isSold)
                        .Set("Position", ticketObject.Position);

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
    }
}