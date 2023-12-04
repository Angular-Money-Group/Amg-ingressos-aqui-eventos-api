using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRowRepository 
    {
        Task<string> SaveRowAsync<T>(StatusTicketsRow ticketRow);
        Task<object> EditTicketsRowAsync<T>(string id, StatusTicketsRow ticketRow);
        Task<StatusTicketsRow> GetCourtesyStatusById<T>(string id);
    }
}