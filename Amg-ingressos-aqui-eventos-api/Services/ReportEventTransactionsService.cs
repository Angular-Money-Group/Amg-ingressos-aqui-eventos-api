using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Dto.report;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Utils;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class ReportEventTransactionsService : IReportEventTransactions
    {
        private readonly MessageReturn _messageReturn;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<ReportEventTransactionsService> _logger;

        public ReportEventTransactionsService(
            IEventRepository eventRepository,
            ILogger<ReportEventTransactionsService> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> ProcessReportEventTransactions(string idOrganizer)
        {
            try
            {
                if (string.IsNullOrEmpty(idOrganizer))
                    throw new ReportException("Id Lote é Obrigatório.");

                idOrganizer.ValidateIdMongo();

                List<EventComplet> dataTicket = await _eventRepository.GetFilterWithTickets<EventComplet>(string.Empty, idOrganizer);
                var dataDto = new EventCompletWithTransactionDto().ModelListToDtoList(dataTicket);

                List<EventComplet> dataTransaction = await _eventRepository.GetFilterWithTransactions<EventComplet>(string.Empty, idOrganizer);
                var dataTransactionDto = new EventCompletWithTransactionDto().ModelListToDtoList(dataTransaction);
                
                var ReportTransactionsDto = ProcessEvent(dataDto, dataTransactionDto, string.Empty);

                _messageReturn.Data = ReportTransactionsDto;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTransactions), "relatorio transações"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> ProcessReportEventTransactionsDetail(string idEvent, string idVariant, string idOrganizer)
        {
            try
            {
                if (string.IsNullOrEmpty(idEvent))
                    throw new ReportException("Id Evento é Obrigatório.");
                idEvent.ValidateIdMongo();

                
                List<EventComplet> dataTickets = await _eventRepository.GetFilterWithTickets<EventComplet>(idEvent, string.Empty);
                var dataTicketsDto = new EventCompletWithTransactionDto().ModelListToDtoList(dataTickets);

                List<EventComplet> dataTransactions = await _eventRepository.GetFilterWithTransactions<EventComplet>(idEvent, idOrganizer);
                var dataTransactionDto = new EventCompletWithTransactionDto().ModelListToDtoList(dataTransactions);
                
                var ReportTransactionsDto = ProcessEvent(dataTicketsDto, dataTransactionDto, idVariant);

                _messageReturn.Data = ReportTransactionsDto;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTransactionsDetail), "relatorio transações detalhes"), ex);
                throw;
            }
        }

        private ReportTransactionsDto ProcessEvent(List<EventCompletWithTransactionDto> eventDataTickets, List<EventCompletWithTransactionDto> eventDataTransaction, string idVariant)
        {
            List<Transaction> listTransaction = GerenateJoinLists(eventDataTickets, eventDataTransaction, idVariant);

            if (!listTransaction.Any())
                return new ReportTransactionsDto();

            var listCredit = listTransaction.Where(i => i.PaymentMethod.TypePayment == Enum.TypePayment.CreditCard);
            var listDebit = listTransaction.Where(i => i.PaymentMethod.TypePayment == Enum.TypePayment.DebitCard);
            var listPix = listTransaction.Where(i => i.PaymentMethod.TypePayment == Enum.TypePayment.Pix);

            ReportTransactionsDto report = new ReportTransactionsDto()
            {
                Credit = new TransactionsDto()
                {
                    Amount = listCredit.Count(),
                    EventValue = listCredit.Sum(x => x.TotalValue),
                    LiquidValue = listCredit.Sum(x => x.TotalValue) - ((listCredit.Sum(x => x.TotalValue) * 15) / 100),
                    TaxValue = (listCredit.Sum(x => x.TotalValue) * 15) / 100,
                    TotalValue = listCredit.Sum(x => x.TotalValue)
                },
                Debit = new TransactionsDto()
                {
                    Amount = listDebit.Count(),
                    EventValue = listDebit.Sum(x => x.TotalValue),
                    LiquidValue = listDebit.Sum(x => x.TotalValue) - ((listDebit.Sum(x => x.TotalValue) * 15) / 100),
                    TaxValue = (listDebit.Sum(x => x.TotalValue) * 15) / 100,
                    TotalValue = listDebit.Sum(x => x.TotalValue)
                },
                Pix = new TransactionsDto()
                {
                    Amount = listPix.Count(),
                    EventValue = listPix.Sum(x => x.TotalValue),
                    LiquidValue = listPix.Sum(x => x.TotalValue) - ((listPix.Sum(x => x.TotalValue) * 15) / 100),
                    TaxValue = (listPix.Sum(x => x.TotalValue) * 15) / 100,
                    TotalValue = listPix.Sum(x => x.TotalValue)
                }
            };
            report.Total = new TransactionsDto()
            {
                Amount = report.Credit.Amount + report.Debit.Amount + report.Pix.Amount,
                EventValue = report.Credit.EventValue + report.Debit.EventValue + report.Pix.EventValue,
                LiquidValue = report.Credit.LiquidValue + report.Debit.LiquidValue + report.Pix.LiquidValue,
                TaxValue = report.Credit.TaxValue + report.Debit.TaxValue + report.Pix.TaxValue,
                TotalValue = report.Credit.TotalValue + report.Debit.TotalValue + report.Pix.TotalValue
            };
            return report;
        }

        private static List<Transaction> GerenateJoinLists(List<EventCompletWithTransactionDto> eventDataTickets, List<EventCompletWithTransactionDto> eventDataTransaction, string idVariant)
        {
            //get tickets de lotes do evento e variante de filtro
            List<Ticket> listTickets = new();
            if (!string.IsNullOrEmpty(idVariant))
            {
                var listLotes = eventDataTickets?.FirstOrDefault()?.Variants?.Find(i => i.Id == idVariant)?.Lots;
                listLotes?.ForEach(i => { listTickets.AddRange(i.Tickets); });
            }
            else
            {
                List<VariantWithLotDto> listVariant = new List<VariantWithLotDto>();
                List<LotWithTicketDto> listLotes = new List<LotWithTicketDto>();
                eventDataTickets.ForEach(x => { listVariant.AddRange(x.Variants); });
                listVariant.ForEach(x => { listLotes.AddRange(x.Lots); });
                listLotes.ForEach(i => { listTickets.AddRange(i.Tickets); });
            }

            //get tickets transactions
            List<Transaction> listTransactions = new List<Transaction>();
            eventDataTransaction.ForEach(x => { listTransactions.AddRange(x.Transactions); });
            List<TransactionIten> listTransactionItens = new List<TransactionIten>();
            listTransactions.ForEach(x => { listTransactionItens.AddRange(x.TransactionItens); });

            //relacionamento entre ticket e transacoes
            var listTransactionTicktes = from transactionTickets in listTransactionItens
                                         join tickets in listTickets on transactionTickets.IdTicket equals tickets.Id
                                         select transactionTickets;
            //lista de transacao para report
            var listTransaction = from transactionJoin in listTransactionTicktes
                                  join transactions in listTransactions
                                  on transactionJoin.IdTransaction equals transactions.Id
                                  select transactions;
            //filtro de transacoes finalizadas
            listTransaction = listTransaction.Where(x => x.PaymentMethod != null && x.Stage == Enum.StageTransaction.Finished);
            return listTransaction.ToList();
        }
    }
}