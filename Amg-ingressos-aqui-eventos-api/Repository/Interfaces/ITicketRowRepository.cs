using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRowRepository 
    {
        Task<string> SaveRowAsync<T>(StatusTicketsRow ticket);
        Task<object> UpdateTicketsRowAsync<T>(string id, StatusTicketsRow ticket);
        Task<StatusTicketsRow> GetCourtesyStatusById<T>(string id);
    }
}