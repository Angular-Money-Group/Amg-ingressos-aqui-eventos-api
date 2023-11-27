using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Infra;

using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Exceptions;

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

        public async Task<string> SaveRowAsync<T1>(StatusTicketsRow ticketRow)
        {
            try
            {
                if (ticketRow == null || string.IsNullOrEmpty(ticketRow.ToString()))
                    throw new SaveException("TicketRow é obrigatório");

                await _ticketStatusCollection.InsertOneAsync(ticketRow);
                return ticketRow.Id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<StatusTicketsRow> GetCourtesyStatusById<T1>(string id)
        {
            try
            {
                if (id == null || string.IsNullOrEmpty(id))
                    throw new GetException("id ticketRow é obrigatório");

                var filter = Builders<StatusTicketsRow>.Filter.Eq("_id", ObjectId.Parse(id));
                var pResult = await _ticketStatusCollection.FindAsync(filter).Result.ToListAsync();
                return pResult.FirstOrDefault() ??
                throw new Exception("Cortesia não encontrada.");

            }
            catch
            {
                throw;
            }
        }

        public async Task<object> UpdateTicketsRowAsync<T1>(string id, StatusTicketsRow ticket)
        {
            try
            {
                if (id == null || string.IsNullOrEmpty(id))
                    throw new GetException("id statusTicketRow é obrigatório");
                if (ticket == null)
                    throw new GetException("status ticket row é obrigatório");

                // Cria um filtro para buscar tickets com o ID do lot especificado
                var filter = Builders<StatusTicketsRow>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<StatusTicketsRow>.Update
                    .Set("TotalTickets", ticket.TotalTickets)
                    .Set("TicketStatus", ticket.TicketStatus);

                // Busca os tickets que correspondem ao filtro
                var result = await _ticketStatusCollection.UpdateOneAsync(filter, update);

                return ticket;
            }
            catch
            {
                throw;
            }
        }
    }
}