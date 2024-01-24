using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportEventTickets _reporteventTickets;
        private readonly IReportEventTransactions _reporteventTransactions;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IReportEventTickets reporteventTickets,
            IReportEventTransactions reporteventTransactions,
            ILogger<ReportService> logger
            )
        {
            _reporteventTickets = reporteventTickets;
            _reporteventTransactions = reporteventTransactions;
            _logger = logger;
        }
        public MessageReturn GetReportEventTicketsDetail(string idEvent, string idVariant)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTicketsDetail(idEvent, idVariant).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetail), "relatorio tickets detalhes"), ex);
                throw;
            }
        }

        public MessageReturn GetReportEventTicketsDetails(string idEvent)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTicketsDetails(idEvent).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetails), "relatorio tickets por evento detalhes"), ex);
                throw;
            }
        }

        public MessageReturn GetReportEventTickets(string idOrganizer)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTickets(idOrganizer).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTickets), "relatorio tickets por evento"), ex);
                throw;
            }
        }
        public MessageReturn GetReportEventTransactionsDetail(string idEvent, string idVariant, string idOrganizer)
        {
            try
            {
                return _reporteventTransactions.ProcessReportEventTransactionsDetail(idEvent, idVariant, idOrganizer).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactionsDetail), "relatorio transacao detalhes"), ex);
                throw;
            }
        }
        public MessageReturn GetReportEventTransactions(string idOrganizer)
        {
            try
            {
                return _reporteventTransactions.ProcessReportEventTransactions(idOrganizer).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactions), "relatorio transacoes evento por organizador"), ex);
                throw;
            }
        }
    }
}