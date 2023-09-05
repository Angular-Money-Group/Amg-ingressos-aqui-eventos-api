using Amg_ingressos_aqui_eventos_api.Dto.report;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventTransactions;
using Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Utils;
using Lot = Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket.Lot;
using Ticket = Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket.Ticket;
using Variant = Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket.Variant;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class ReportEventTransactionsService : IReportEventTransactions
    {
        private MessageReturn _messageReturn= new MessageReturn();
        private IEventRepository _eventRepository;

        public ReportEventTransactionsService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<MessageReturn> ProcessReportEventTransactions(string idOrganizer)
        {
            try
            {
                if (string.IsNullOrEmpty(idOrganizer))
                    throw new SaveTicketException("Id Lote é Obrigatório.");

                idOrganizer.ValidateIdMongo();

                List<GetEventWitTickets> eventDataTickets = await _eventRepository.GetAllEventsWithTickets(string.Empty,idOrganizer);
                List<GetEventTransactions> eventDataTransaction = await _eventRepository.GetAllEventsWithTransactions(string.Empty, idOrganizer);
                
                var ReportTransactionsDto = ProcessEvent(eventDataTickets, eventDataTransaction, string.Empty);


                _messageReturn.Data = ReportTransactionsDto;
                return _messageReturn;
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

        public async Task<MessageReturn> ProcessReportEventTransactionsDetail(string idEvent, string idVariant, string idOrganizer)
        {
            try
            {
                if (string.IsNullOrEmpty(idEvent))
                    throw new SaveTicketException("Id Lote é Obrigatório.");

                idEvent.ValidateIdMongo();

                List<GetEventWitTickets> eventDataTickets = await _eventRepository.GetAllEventsWithTickets(idEvent, string.Empty);
                List<GetEventTransactions> eventDataTransaction = await _eventRepository.GetAllEventsWithTransactions(idEvent, idOrganizer);
                var ReportTransactionsDto = ProcessEvent(eventDataTickets, eventDataTransaction, idVariant);


                _messageReturn.Data = ReportTransactionsDto;
                return _messageReturn;
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

        private ReportTransactionsDto ProcessEvent(List<GetEventWitTickets> eventDataTickets, List<GetEventTransactions> eventDataTransaction, string idVariant)
        {
            List<Transaction> listTransaction = GerenateJoinLists(eventDataTickets, eventDataTransaction, idVariant);

            if (!listTransaction.Any())
                return new ReportTransactionsDto();

            var listCredit = listTransaction.Where(i => i.PaymentMethod.TypePayment == Enum.TypePaymentEnum.CreditCard);
            var listDebit = listTransaction.Where(i => i.PaymentMethod.TypePayment == Enum.TypePaymentEnum.DebitCard);
            var listPix = listTransaction.Where(i => i.PaymentMethod.TypePayment == Enum.TypePaymentEnum.Pix);

            ReportTransactionsDto report = new ReportTransactionsDto()
            {
                Credito = new TransactionsDto()
                {
                    Quantidade = listCredit.Count(),
                    ValorEvento = listCredit.Sum(x => x.TotalValue),
                    ValorLiquido = listCredit.Sum(x => x.TotalValue) - ((listCredit.Sum(x => x.TotalValue) * 15) / 100),
                    ValorTaxas = (listCredit.Sum(x => x.TotalValue) * 15) / 100,
                    ValorTotal = listCredit.Sum(x => x.TotalValue)
                },
                Debito = new TransactionsDto()
                {
                    Quantidade = listDebit.Count(),
                    ValorEvento = listDebit.Sum(x => x.TotalValue),
                    ValorLiquido = listDebit.Sum(x => x.TotalValue) - ((listDebit.Sum(x => x.TotalValue) * 15) / 100),
                    ValorTaxas = (listDebit.Sum(x => x.TotalValue) * 15) / 100,
                    ValorTotal = listDebit.Sum(x => x.TotalValue)
                },
                Pix = new TransactionsDto()
                {
                    Quantidade = listPix.Count(),
                    ValorEvento = listPix.Sum(x => x.TotalValue),
                    ValorLiquido = listPix.Sum(x => x.TotalValue) - ((listPix.Sum(x => x.TotalValue) * 15) / 100),
                    ValorTaxas = (listPix.Sum(x => x.TotalValue) * 15) / 100,
                    ValorTotal = listPix.Sum(x => x.TotalValue)
                }
            };
            report.Total = new TransactionsDto()
            {
                Quantidade = report.Credito.Quantidade + report.Debito.Quantidade + report.Pix.Quantidade,
                ValorEvento = report.Credito.ValorEvento + report.Debito.ValorEvento + report.Pix.ValorEvento,
                ValorLiquido = report.Credito.ValorLiquido + report.Debito.ValorLiquido + report.Pix.ValorLiquido,
                ValorTaxas = report.Credito.ValorTaxas + report.Debito.ValorTaxas + report.Pix.ValorTaxas,
                ValorTotal = report.Credito.ValorTotal + report.Debito.ValorTotal + report.Pix.ValorTotal
            };
            return report;
        }

        private static List<Transaction> GerenateJoinLists(List<GetEventWitTickets> eventDataTickets, List<GetEventTransactions> eventDataTransaction, string idVariant)
        {
            //get tickets de lotes do evento e variante de filtro
            List<Ticket> listTickets = new();
            if(!string.IsNullOrEmpty(idVariant)){
                var listLotes = eventDataTickets.FirstOrDefault().Variant.FirstOrDefault(i => i._id == idVariant).Lot;
                listLotes.ForEach(i => { listTickets.AddRange(i.ticket); });
            }
            else{
                List<Variant> listVariant = new List<Variant>();
                eventDataTickets.ForEach(x =>
                {
                    listVariant.AddRange(x.Variant);
                });
                List<Lot> listLotes = new List<Lot>();
                listVariant.ForEach(x =>
                {
                    listLotes.AddRange(x.Lot);
                });
                listLotes.ForEach(i => { listTickets.AddRange(i.ticket); });
            }

            //get tickets transactions
            List<Transaction> listTransactions = new List<Transaction>();
            eventDataTransaction.ForEach(x =>
            {
                listTransactions.AddRange(x.Transaction);
            });
            List<TransactionIten> listTransactionItens = new List<TransactionIten>();
            listTransactions.ForEach(x =>
            {
                listTransactionItens.AddRange(x.TransactionItens);
            });
            //relacionamento entre ticket e transacoes
            var listTransactionTicktes = from transactionTickets in listTransactionItens
                                      join tickets in listTickets on transactionTickets.IdTicket equals tickets._id
                                      select transactionTickets;
            //lista de transacao para report
            var listTransaction = from transactionJoin in listTransactionTicktes
                                  join transactions in listTransactions
                                  on transactionJoin.IdTransaction equals transactions._id
                                  select transactions;
            //filtro de transacoes finalizadas
            listTransaction = listTransaction.Where(x => x.PaymentMethod != null && x.Stage == Enum.StageTransactionEnum.Finished);
            return listTransaction.ToList();
        }
    }
}