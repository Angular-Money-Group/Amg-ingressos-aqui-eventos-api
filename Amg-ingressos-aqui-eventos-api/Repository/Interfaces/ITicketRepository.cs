using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRepository 
    {
        Task<object> Save<T>(object ticket);
        Task<object> DeleteMany<T>(List<string> idLot);
        Task<object> DeleteByLot<T>(string idLot);
        Task<List<Ticket>> GetTickets<T>(Ticket ticket);
        Task<object> GetTicketByIdDataUser<T>(string id);
        Task<List<string>> GetTicketsByLot<T>(string idLot);
        Task<object> UpdateTicketsAsync<T>(string id, Ticket ticket);
    }
}