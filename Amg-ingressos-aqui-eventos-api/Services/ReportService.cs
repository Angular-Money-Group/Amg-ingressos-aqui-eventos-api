using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

namespace Amg_ingressos_aqui_eventos_api.Services
{

    public class ReportService : IReportService
    {
        private IReportEventTickets _reporteventTickets;
        private IReportEventTransactions _reporteventTransactions;
        private MessageReturn _messageReturn = new MessageReturn();

        public ReportService(IReportEventTickets reporteventTickets,IReportEventTransactions reporteventTransactions){
            _reporteventTickets = reporteventTickets;
            _reporteventTransactions = reporteventTransactions;
        }
        public async Task<MessageReturn> GetReportEventTicketsDetail(string idEvent, string idVariant)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTicketsDetail(idEvent, idVariant).Result;
            }
            catch (SaveTicketException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> GetReportEventTicketsDetails(string idEvent)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTicketsDetails(idEvent).Result;
            }
            catch (SaveTicketException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<MessageReturn> GetReportEventTickets(string idOrganizer)
        {
            try
            {
                return _reporteventTickets.ProcessReportEventTickets(idOrganizer).Result;
            }
            catch (SaveTicketException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    
        public async Task<MessageReturn> GetReportEventTransactionsDetail(string idEvent, string idVariant,string idOrganizer)
        {
            try
            {
                return _reporteventTransactions.ProcessReportEventTransactionsDetail(idEvent,idVariant,idOrganizer).Result;
            }
            catch (SaveTicketException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<MessageReturn> GetReportEventTransactions(string idOrganizer)
        {
            try
            {
                return _reporteventTransactions.ProcessReportEventTransactions(idOrganizer).Result;
            }
            catch (SaveTicketException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}