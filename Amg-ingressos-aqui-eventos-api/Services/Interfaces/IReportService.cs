using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IReportService
    {
        Task<MessageReturn> GetReportEventTicketsDetail(string idEvent,string idVariant);
        Task<MessageReturn> GetReportEventTickets(string idEvent);
    }
}