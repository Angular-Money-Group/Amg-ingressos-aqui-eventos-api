using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRowRepository 
    {
        Task<string> SaveRowAsync(StatusTicketsRow ticketRow);
        Task<object> EditTicketsRowAsync(string id, StatusTicketsRow ticketRow);
        Task<StatusTicketsRow> GetCourtesyStatusById(string id);
    }
}