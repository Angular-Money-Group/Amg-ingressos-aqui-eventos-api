using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRepository 
    {
        Task<object> Save<T>(object ticket);
        Task<object> DeleteMany<T>(List<string> idLot);
        Task<object> Delete<T>(string idLot);
        Task<List<Ticket>> GetTickets<T>(Ticket ticket);
        Task<List<string>> GetTicketsByLot<T>(string id);
        Task<object> UpdateTicketsAsync<T>(string id, Ticket ticket);
    }
}