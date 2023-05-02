using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ITicketRepository 
    {
        Task<object> Save<T>(object ticket);
        Task<List<Ticket>> GetUserTickets<T>(string id);
    }
}