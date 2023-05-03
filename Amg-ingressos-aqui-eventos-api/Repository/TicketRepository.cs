using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using System.Data;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;

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
    }
}