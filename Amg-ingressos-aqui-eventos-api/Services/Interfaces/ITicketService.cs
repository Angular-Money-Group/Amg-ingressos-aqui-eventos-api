using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface ITicketService
    {
        Task<MessageReturn> SaveAsync(Ticket ticket);
        Task<MessageReturn> GetByUser(string idUser);
        Task<MessageReturn> GetByUserAndEvent(string idUser, string idEvent);
        Task<MessageReturn> GetTicketsByLot(string idLot);
        Task<MessageReturn> GetRemainingByLot(string idLot);
        Task<MessageReturn> EditAsync(string id, Ticket ticket);
        Task<MessageReturn> GetById(string id);
        Task<MessageReturn> GetCourtesyStatusById(string id);
        Task<MessageReturn> GetByIdWithDataUser(string id);
        Task<MessageReturn> GetByIdWithDataEvent(string id);
        Task<MessageReturn> DeleteTicketsByLot(string lotId);
        Task<MessageReturn> SaveManyAsync(List<Ticket> listTicket);
        MessageReturn SendCourtesyTickets(CourtesyTicketDto courtesyTicket);
        MessageReturn ReSendCourtesyTickets(string rowId, string variantId);
    }
}