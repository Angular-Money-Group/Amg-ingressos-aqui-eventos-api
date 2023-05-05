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
    public class TicketRepository<T> : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _variantCollection;
        private readonly MongoClient _mongoClient;
        public TicketRepository(IDbConnection<Ticket> dbConnection)
        {
            _variantCollection = dbConnection.GetConnection("tickets");
            _mongoClient = dbConnection.GetClient();
        }

        public async Task<object> Save<T>(object ticket)
        {

            try
            {
                await _variantCollection.InsertOneAsync(ticket as Ticket);
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

        public async Task<List<Ticket>> GetTicketByUser<T>(string id)
        {
            try
            {
                // Cria um filtro para buscar tickets com o ID do usuário especificado
                var filter = Builders<Ticket>.Filter.Eq("IdUser", id);

                // Busca os tickets que correspondem ao filtro
                var result = await _variantCollection.Find(filter).ToListAsync();
                if (!result.Any() || result.Count == 0)
                    throw new FindTicketByUserException("Tickets não encontrados");

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
        public async Task<List<Ticket>> GetTicketsRemaining<T>(string id)
        {
            try
            {
                // Cria um filtro para buscar tickets com o ID do lot especificado
                var filter = Builders<Ticket>.Filter.Eq("IdLot", id) & Builders<Ticket>.Filter.Exists("IdUser", false);

                // Busca os tickets que correspondem ao filtro
                var result = await _variantCollection.Find(filter).ToListAsync();
                if (!result.Any() || result.Count == 0)
                    throw new GetRemeaningTicketsExepition("Tickets não encontrados");

                return result;
            }
            catch (GetRemeaningTicketsExepition ex)
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
                var result = await _variantCollection.UpdateOneAsync(filter, update);

                if (result.ModifiedCount == 0)
                {
                    throw new NotModificateTicketsExeption("O ticket não foi atualizado");
                }
                
                return await _variantCollection.Find(filter).ToListAsync();
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