using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IReportEventTickets
    {
        Task<MessageReturn> ProcessReportEventTickets(string idOrganizer);
        Task<MessageReturn> ProcessReportEventTicketsDetail(string idEvent,string idVariant);
        Task<MessageReturn> ProcessReportEventTicketsDetails(string idEvent);
        
    }
}