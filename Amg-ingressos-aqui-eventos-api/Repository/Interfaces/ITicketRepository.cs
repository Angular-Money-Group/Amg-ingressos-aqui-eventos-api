using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRepository
    {
        Task<object> SaveAsync<T>(object ticket);
        Task<object> DeleteMany<T>(List<string> listId);
        Task<object> DeleteByLot<T>(string idLot);
        Task<List<Ticket>> GetTickets<T>(Ticket ticket);
        Task<List<T>> GetByUser<T>(string idUser);
        Task<List<T>> GetByIdWithDataUser<T>(string id);
        Task<List<T>> GetByIdWithDataEvent<T>(string id);
        Task<Ticket> GetById<T>(string id);
        Task<List<string>> GetTicketsByLot<T>(string idLot);
        Task<object> EditAsync<T>(string id, Ticket ticket);
        Task<object> SaveMany(List<Ticket> lstTicket);
        Task<object> BurnTicketsAsync<T>(string id, int status);
    }
}