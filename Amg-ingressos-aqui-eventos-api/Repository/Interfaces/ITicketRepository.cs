using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRepository 
    {
        Task<object> SaveAsync<T>(object ticket);
        Task<object> DeleteMany<T>(List<string> idLot);
        Task<object> DeleteByLot<T>(string idLot);
        Task<List<Ticket>> GetTickets<T>(Ticket ticket);
        Task<List<GetTicketDataEvent>> GetTicketsByUser<T>(string idUser);
        Task<object> GetTicketByIdDataUser<T>(string id);
        Task<object> GetTicketByIdDataEvent<T>(string id);
        Task<Ticket> GetById<T>(string id);
        Task<List<string>> GetTicketsByLot<T>(string idLot);
        Task<object> UpdateTicketsAsync<T>(string id, Ticket ticket);
        Task<object> SaveMany(List<Ticket> lstTicket);
        Task<object> BurnTicketsAsync<T>(string id, int status);
    }
}