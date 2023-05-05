using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface ITicketService
    {
        Task<MessageReturn> SaveAsync(Ticket ticket);
        Task<MessageReturn> GetTicketByUser(string id);
        Task<MessageReturn> GetTicketsRemaining(string id);
        Task<MessageReturn> UpdateTicketsAsync(string id, Ticket ticketObject);
    }
}