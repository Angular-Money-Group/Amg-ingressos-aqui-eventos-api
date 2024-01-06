using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRepository : ICrudRepository<Ticket>
    {
        Task<bool> DeleteMany(List<string> listId);
        Task<bool> DeleteByLot(string idLot);
        Task<List<T>> GetTickets<T>(Ticket ticket);
        Task<List<T>> GetByUser<T>(string idUser);
        Task<List<T>> GetByIdWithDataUser<T>(string id);
        Task<List<T>> GetByIdWithDataEvent<T>(string id);
        Task<List<string>> GetTicketsByLot(string idLot);
        Task<bool> SaveMany(List<Ticket> lstTicket);
        Task<bool> BurnTicketsAsync(string id, int status);
        Task<long> GetCountTicketsNoUser(string idLot);
    }
}