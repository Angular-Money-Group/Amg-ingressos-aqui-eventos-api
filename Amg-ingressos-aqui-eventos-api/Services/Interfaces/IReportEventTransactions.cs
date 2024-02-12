using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IReportEventTransactions
    {
        Task<MessageReturn> ProcessReportEventTransactions(string idOrganizer);
        Task<MessageReturn> ProcessReportEventTransactionsDetail(string idEvent, string idVariant,string idOrganizer);
    }
}