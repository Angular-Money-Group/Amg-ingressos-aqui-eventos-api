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
    public class TicketRowRepository<T> : ITicketRowRepository
    {
        private readonly IMongoCollection<StatusTicketsRow> _ticketStatusCollection;

        public TicketRowRepository(
            IDbConnection<StatusTicketsRow> dbConnectionCourtesy
        )
        {
            _ticketStatusCollection = dbConnectionCourtesy.GetConnection("statusCourtesyTickets");
        }

        public async Task<string> SaveRowAsync<T>(StatusTicketsRow tickets)
        {
            try
            {
                await _ticketStatusCollection.InsertOneAsync(tickets);
                return tickets.Id;
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

        public async Task<StatusTicketsRow> GetCourtesyStatusById<T>(string id)
        {
            try
            {
                var filter = Builders<StatusTicketsRow>.Filter.Eq("_id", ObjectId.Parse(id));
                var pResult = _ticketStatusCollection.Find(filter).ToList();
                return pResult.FirstOrDefault();

            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> UpdateTicketsRowAsync<T>(string id, StatusTicketsRow ticket)
        {
            try
            {
                // Cria um filtro para buscar tickets com o ID do lot especificado
                var filter = Builders<StatusTicketsRow>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<StatusTicketsRow>.Update
                    .Set("TotalTickets", ticket.TotalTickets)
                    .Set("TicketStatus", ticket.TicketStatus);

                // Busca os tickets que correspondem ao filtro
                var result = await _ticketStatusCollection.UpdateOneAsync(filter, update);

                return ticket;
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