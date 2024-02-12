using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IReportService
    {
        MessageReturn GetReportEventTickets(string idOrganizer);
        MessageReturn GetReportEventTicketsDetail(string idEvent,string idVariant);
        MessageReturn GetReportEventTicketsDetails(string idEvent);
        MessageReturn GetReportEventTransactions(string idOrganizer);
        MessageReturn GetReportEventTransactionsDetail(string idEvent, string idVariant,string idOrganizer);
    }
}