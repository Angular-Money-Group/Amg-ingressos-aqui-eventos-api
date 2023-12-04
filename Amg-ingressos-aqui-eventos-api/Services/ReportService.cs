using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportEventTickets _reporteventTickets;
        private readonly IReportEventTransactions _reporteventTransactions;
        private readonly ILogger<ReportService> _logger;
        private readonly MessageReturn _messageReturn;

        public ReportService(
            IReportEventTickets reporteventTickets,
            IReportEventTransactions reporteventTransactions,
            ILogger<ReportService> logger
            )
        {
            _reporteventTickets = reporteventTickets;
            _reporteventTransactions = reporteventTransactions;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }
        public MessageReturn GetReportEventTicketsDetail(string idEvent, string idVariant)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTicketsDetail(idEvent, idVariant).Result;
            }
            catch (ReportException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetail), "relatorio tickets detalhes"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetail), "relatorio tickets detalhes"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetail), "relatorio tickets detalhes"));
                throw;
            }
        }

        public MessageReturn GetReportEventTicketsDetails(string idEvent)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTicketsDetails(idEvent).Result;
            }
            catch (ReportException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetails), "relatorio tickets por evento detalhes"), idEvent);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetails), "relatorio tickets por evento detalhes"), idEvent);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTicketsDetails), "relatorio tickets por evento detalhes"), idEvent);
                throw;
            }
        }

        public MessageReturn GetReportEventTickets(string idOrganizer)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTickets(idOrganizer).Result;
            }
            catch (ReportException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTickets), "relatorio tickets por evento"), idOrganizer);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTickets), "relatorio tickets por evento"), idOrganizer);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTickets), "relatorio tickets por evento"), idOrganizer);
                throw;
            }
        }
        public MessageReturn GetReportEventTransactionsDetail(string idEvent, string idVariant, string idOrganizer)
        {
            try
            {
                return _reporteventTransactions.ProcessReportEventTransactionsDetail(idEvent, idVariant, idOrganizer).Result;
            }
            catch (ReportException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactionsDetail), "relatorio transacao detalhes"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactionsDetail), "relatorio transacao detalhes"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactionsDetail), "relatorio transacao detalhes"));
                throw;
            }
        }
        public MessageReturn GetReportEventTransactions(string idOrganizer)
        {
            try
            {
                return _reporteventTransactions.ProcessReportEventTransactions(idOrganizer).Result;
            }
            catch (ReportException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactions), "relatorio transacoes evento por organizador"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactions), "relatorio transacoes evento por organizador"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(GetReportEventTransactions), "relatorio transacoes evento por organizador"));
                throw;
            }
        }
    }
}