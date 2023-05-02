using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using System.Data;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TicketRepository<T> : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _variantCollection;
        public TicketRepository(IDbConnection<Ticket> dbconnection)
        {
            _variantCollection = dbconnection.GetConnection("tickets");
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
                if (!result.Any() || result.Count == 0 )
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
    }
}