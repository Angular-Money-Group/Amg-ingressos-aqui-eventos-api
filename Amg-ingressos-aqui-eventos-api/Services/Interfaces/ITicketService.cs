using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface ITicketService
    {
        Task<MessageReturn> SaveAsync(Ticket ticket);
        Task<MessageReturn> GetTicketByUser(string idUser);
        Task<MessageReturn> GetTicketsByLot(string idLot);
        Task<MessageReturn> GetTicketsRemainingByLot(string idLot);
        Task<MessageReturn> UpdateTicketsAsync(string id, Ticket ticketObject);
        Task<MessageReturn> GetTicketById(string id);
        Task<MessageReturn> GetCourtesyStatus(string id);
        Task<MessageReturn> GetTicketByIdDataUser(string id);
        Task<MessageReturn> GetTicketByIdDataEvent(string id);
        Task<MessageReturn> DeleteTicketsByLot(string LotId);
        Task<MessageReturn> SaveManyAsync(List<Ticket> ticket);
        Task<MessageReturn> SendCourtesyTickets(GenerateCourtesyTicketDto courtesyTicketDto);
    }
}