using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IReportService
    {
        Task<MessageReturn> GetReportEventTickets(string idOrganizer);
        Task<MessageReturn> GetReportEventTicketsDetail(string idEvent,string idVariant);
        Task<MessageReturn> GetReportEventTicketsDetails(string idEvent);
        Task<MessageReturn> GetReportEventTransactions(string idOrganizer);
        Task<MessageReturn> GetReportEventTransactionsDetail(string idEvent, string idVariant,string idOrganizer);
    }
}