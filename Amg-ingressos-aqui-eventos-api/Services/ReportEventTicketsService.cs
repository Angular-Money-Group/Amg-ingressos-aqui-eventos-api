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
    public class ReportEventTicketsService : IReportEventTickets
    {
        private readonly MessageReturn _messageReturn;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<ReportEventTicketsService> _logger;

        public ReportEventTicketsService(
            IEventRepository eventRepository,
            ILogger<ReportEventTicketsService> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> ProcessReportEventTickets(string idOrganizer)
        {
            try
            {
                if (string.IsNullOrEmpty(idOrganizer))
                    throw new ReportException("Id Organizador é Obrigatório.");

                idOrganizer.ValidateIdMongo();
                List<EventComplet> dataTickets = await _eventRepository.GetFilterWithTickets<EventComplet>(string.Empty, idOrganizer);
                var dataDto = new EventCompletWithTransactionDto().ModelListToDtoList(dataTickets);


                List<ReportEventOrganizerDetailDto> reports = new();
                dataDto.ForEach(x =>
                {
                    reports.Add(ProcessEvent(x, string.Empty));
                });
                _messageReturn.Data = !reports.Any() ? new ReportEventOrganizerDto() :
                new ReportEventOrganizerDto()
                {
                    AmountTicket = reports.Sum(x => x.AmountTicket),
                    Cortesys = new CourtesyReportDto()
                    {
                        Entregues = new()
                        {
                            Percent = reports.Sum(i => i.Cortesy.Entregues.Percent),
                            Amount = reports.Sum(i => i.Cortesy.Entregues.Amount),
                        },
                        Restantes = new()
                        {
                            Percent = reports.Sum(i => i.Cortesy.Restantes.Percent),
                            Amount = reports.Sum(i => i.Cortesy.Restantes.Amount),
                            Tax = 15,
                            ReceiveValue = reports.Sum(i => i.Cortesy.Restantes.ReceiveValue),
                            TotalValue = reports.Sum(i => i.Cortesy.Restantes.TotalValue)
                        }
                    },
                    Tickets = new TicketsReportDto()
                    {
                        Remaining = new()
                        {
                            Percent =
                                (
                                    Convert.ToDouble(
                                        reports.Sum(i => i.Tickets.Remaining.Amount)
                                    ) / Convert.ToDouble(reports.Sum(x => x.AmountTicket))
                                ) * 100,
                            Amount = reports.Sum(i => i.Tickets.Remaining.Amount),
                            Tax = reports.Sum(i => i.Tickets.Remaining.Tax),
                            ReceiveValue = reports.Sum(i => i.Tickets.Remaining.ReceiveValue),
                            TotalValue = reports.Sum(i => i.Tickets.Remaining.TotalValue)
                        },
                        Sold = new()
                        {
                            Percent =
                                (
                                    Convert.ToDouble(
                                        reports.Sum(i => i.Tickets.Sold.Amount)
                                    ) / Convert.ToDouble(reports.Sum(x => x.AmountTicket))
                                ) * 100,
                            Amount = reports.Sum(i => i.Tickets.Sold.Amount),
                            Tax = reports.Sum(i => i.Tickets.Sold.Tax),
                            ReceiveValue = reports.Sum(i => i.Tickets.Sold.ReceiveValue),
                            TotalValue = reports.Sum(i => i.Tickets.Sold.TotalValue)
                        }
                    }
                };
            }
            catch (ReportException ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTickets), "Eventos por ticket"), ex);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTickets), "Eventos por ticket"), ex);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTickets), "Eventos por ticket"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> ProcessReportEventTicketsDetail(string idEvent, string idVariant)
        {
            try
            {
                if (string.IsNullOrEmpty(idEvent))
                    throw new ReportException("Id Evento é Obrigatório.");
                idEvent.ValidateIdMongo();
                if (string.IsNullOrEmpty(idVariant))
                    throw new ReportException("Id Variante é Obrigatório.");
                idVariant.ValidateIdMongo();

                List<EventComplet> dataTickets = await _eventRepository.GetFilterWithTickets<EventComplet>(idEvent, string.Empty);
                var dataDto = new EventCompletWithTransactionDto().ModelListToDtoList(dataTickets);

                var eventDataProcess = dataDto.Find(i => i.Id == idEvent) ?? throw new ReportException("Dados não pode ser null");
                _messageReturn.Data = ProcessEvent(eventDataProcess, idVariant);
                return _messageReturn;

            }
            catch (ReportException ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetail), "Eventos por ticket"), ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetail), "Eventos por ticket"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> ProcessReportEventTicketsDetails(string idEvent)
        {
            try
            {
                if (string.IsNullOrEmpty(idEvent))
                    throw new ReportException("Id Evento é Obrigatório.");
                idEvent.ValidateIdMongo();
                List<EventComplet> dataTickets = await _eventRepository.GetFilterWithTickets<EventComplet>(idEvent, string.Empty);
                var dataDto = new EventCompletWithTransactionDto().ModelListToDtoList(dataTickets);
                var eventDataProcess = dataDto.Find(i => i.Id == idEvent) ?? throw new ReportException("Dados não pode ser null");
                _messageReturn.Data = ProcessEvent(eventDataProcess, string.Empty);
                return _messageReturn;
            }
            catch (ReportException ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetails), "Eventos por ticket"), ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetails), "Eventos por ticket"), ex);
                throw;
            }
        }

        private ReportEventOrganizerDetailDto ProcessEvent(EventCompletWithTransactionDto eventData, string? idVariant)
        {
            //filtra variante a pser processada
            var listVariantProcess = string.IsNullOrEmpty(idVariant) ?
                eventData.Variants :
                eventData.Variants.Where(i => i.Id == idVariant).ToList();
            eventData.Variants = listVariantProcess;

            var variants = ProcessVariant(eventData);

            return new ReportEventOrganizerDetailDto()
            {
                Name = eventData.Name,
                AmountTicket = variants.Sum(i => i.AmountTickets),
                Tickets = new TicketsReportDto()
                {
                    Sold = new SoldDto()
                    {
                        Percent = (variants.Sum(i => i.Tickets.Sold.Amount) == 0 && variants.Sum(i => i.AmountTickets) == 0) ? 100 :
                            (
                                Convert.ToDouble(variants.Sum(i => i.Tickets.Sold.Amount))
                                / Convert.ToDouble(variants.Sum(i => i.AmountTickets))
                            ) * 100,
                        Amount = variants.Sum(i => i.Tickets.Sold.Amount),
                        Tax = 15,
                        ReceiveValue = variants.Sum(i => i.Tickets.Sold.ReceiveValue),
                        TotalValue = variants.Sum(i => i.Tickets.Sold.TotalValue)
                    },
                    Remaining = new RemainingDto()
                    {
                        Percent = (variants.Sum(i => i.Tickets.Remaining.Amount) == 0 && variants.Sum(i => i.AmountTickets) == 0) ? 100 :
                            (
                                Convert.ToDouble(variants.Sum(i => i.Tickets.Remaining.Amount)) / Convert.ToDouble(variants.Sum(i => i.AmountTickets))
                            ) * 100,
                        Amount = variants.Sum(i => i.Tickets.Remaining.Amount),
                        Tax = 15,
                        ReceiveValue = variants.Sum(i => i.Tickets.Remaining.ReceiveValue),
                        TotalValue = variants.Sum(i => i.Tickets.Remaining.TotalValue)
                    }
                },
                Cortesy = VerifyCortesy(variants, eventData),
                Variant = string.IsNullOrEmpty(idVariant) ? new VariantReportDto()
                    : variants?.Find(x => x.Name == eventData?.Variants?.Find(i => i.Id == idVariant)?.Name) ?? new VariantReportDto()
            };
        }

        private CourtesyReportDto VerifyCortesy(List<VariantReportDto> variants, EventCompletWithTransactionDto eventData)
        {
            if (eventData.Courtesy.RemainingCourtesy.Count == 0) return new CourtesyReportDto();
            return new CourtesyReportDto()
            {
                Entregues = new DeliveredReportDto()
                {
                    Percent = (variants.Sum(i => i.Cortesys.Entregues.Amount) == 0 && variants.Sum(i => i.Cortesys.Restantes.Amount) == 0) ? 100 :
                            (
                                Convert.ToDouble(
                                    variants.Sum(i => i.Cortesys.Entregues.Amount)
                                ) / Convert.ToDouble(variants.Sum(i => i.Cortesys.Restantes.Amount))
                            ) * 100,
                    Amount = variants.Sum(i => i.Cortesys.Entregues.Amount)
                },
                Restantes = new RemainingDto()
                {
                    Percent = (variants.Sum(i => i.Cortesys.Entregues.Amount) == 0 && variants.Sum(i => i.Cortesys.Restantes.Amount) == 0) ? 100 :
                            (
                                Convert.ToDouble(
                                    variants.Sum(i => i.Cortesys.Entregues.Amount)
                                ) / Convert.ToDouble(variants.Sum(i => i.Cortesys.Restantes.Amount))
                            ) * 100,
                    Amount = variants.Sum(i => i.Cortesys.Restantes.Amount)
                }
            };
        }

        private List<VariantReportDto> ProcessVariant(EventCompletWithTransactionDto eventData)
        {
            List<VariantReportDto> variantList = new List<VariantReportDto>();

            eventData.Variants.ForEach(x =>
            {
                var cortesyHistory = eventData.Courtesy.CourtesyHistory
                .Where(i => i.Variant == x.Name)
                .ToList();
                var cortesyRemaining = eventData.Courtesy.RemainingCourtesy
                .Where(i => i.VariantId == x.Id)
                .ToList();
                var reportCortesia = ProcessCortesy(cortesyHistory, cortesyRemaining);
                var lots = ProcessLotes(x.Lots);


                var variantDto = new VariantReportDto()
                {
                    Name = x.Name,
                    AmountTickets = lots.Sum(i => i.AmountTicket),
                    Tickets = new TicketsReportDto()
                    {
                        Sold = new SoldDto()
                        {
                            Percent = (lots.Sum(i => i.Tickets.Sold.Amount) == 0 && lots.Sum(i => i.AmountTicket) == 0) ? 100 :
                                (
                                    Convert.ToDouble(lots.Sum(i => i.Tickets.Sold.Amount))
                                    / Convert.ToDouble(lots.Sum(i => i.AmountTicket))
                                ) * 100,
                            Amount = lots.Sum(i => i.Tickets.Sold.Amount),
                            Tax = 15,
                            ReceiveValue = lots.Sum(i => i.Tickets.Sold.ReceiveValue),
                            TotalValue = lots.Sum(i => i.Tickets.Sold.TotalValue)
                        },
                        Remaining = new RemainingDto()
                        {
                            Percent = (lots.Sum(i => i.Tickets.Remaining.Amount) == 0 && lots.Sum(i => i.AmountTicket) == 0) ? 100 :
                                (
                                    Convert.ToDouble(
                                        lots.Sum(i => i.Tickets.Remaining.Amount)
                                    ) / Convert.ToDouble(lots.Sum(i => i.AmountTicket))
                                ) * 100,
                            Amount = lots.Sum(i => i.Tickets.Remaining.Amount),
                            Tax = 15,
                            ReceiveValue = lots.Sum(i => i.Tickets.Remaining.ReceiveValue),
                            TotalValue = lots.Sum(i => i.Tickets.Remaining.TotalValue)
                        }
                    },
                    Cortesys = reportCortesia,
                    Lots = lots
                };
                variantList.Add(variantDto);
            });
            return variantList;
        }

        private CourtesyReportDto ProcessCortesy(
            List<CourtesyHistory> cortesyHistory,
            List<RemainingCourtesy> cortesyRemaining
        )
        {
            return new CourtesyReportDto()
            {
                Entregues =
                    cortesyRemaining.Sum(x => x.Quantity) != 0
                        ? new DeliveredReportDto()
                        {
                            Percent = (cortesyHistory.Sum(x => x.Quantity) == 0 && cortesyRemaining.Sum(x => x.Quantity) == 0) ? 100 :
                                (
                                    Convert.ToDouble(
                                        cortesyHistory.Sum(x => x.Quantity)
                                            / Convert.ToDouble(
                                                cortesyRemaining.Sum(x => x.Quantity)
                                            )
                                    )
                                ) * 100,
                            Amount = cortesyHistory.Sum(x => x.Quantity)
                        }
                        : new DeliveredReportDto()
                        {
                            Percent = 100,
                            Amount = cortesyHistory.Sum(x => x.Quantity)
                        },
                Restantes =
                    cortesyRemaining.Sum(x => x.Quantity) != 0
                        ? new RemainingDto()
                        {
                            Percent = (cortesyHistory.Sum(x => x.Quantity) == 0 && cortesyRemaining.Sum(x => x.Quantity) == 0) ? 100 :
                                (
                                    Convert.ToDouble(
                                        (
                                            cortesyRemaining.Sum(x => x.Quantity)
                                            - cortesyHistory.Sum(x => x.Quantity)
                                        )
                                    ) / Convert.ToDouble(cortesyRemaining.Sum(x => x.Quantity))
                                ) * 100,
                            Amount = (
                                cortesyRemaining.Sum(x => x.Quantity)
                                - cortesyHistory.Sum(x => x.Quantity)
                            )
                        }
                        : new RemainingDto()
                        {
                            Percent = 0,
                            Amount = cortesyRemaining.Sum(x => x.Quantity)
                        }
            };
        }

        private List<LotReportDto> ProcessLotes(List<LotWithTicketDto> LotData)
        {
            var listLot = new List<LotReportDto>();
            LotData.ForEach(i =>
            {
                listLot.Add(
                    new LotReportDto()
                    {
                        Name = i.Identificate.ToString(),
                        AmountTicket = i.Tickets.Count,
                        Tickets = new TicketsReportDto()
                        {
                            Sold = new SoldDto()
                            {
                                Percent = (i.Tickets.Count == 0) ? 100 :
                                    (
                                        Convert.ToDouble(i.Tickets.Count(x => x.Status == Enum.StatusTicket.VENDIDO))
                                        / Convert.ToDouble(i.Tickets.Count)
                                    ) * 100,
                                Amount = i.Tickets.Count(x => x.Status == Enum.StatusTicket.VENDIDO),
                                Tax = 15,
                                ReceiveValue =
                                    i.Tickets.Where(x => x.Status == Enum.StatusTicket.VENDIDO).Sum(i => i.Value)
                                    - (
                                        (15 * i.Tickets.Where(x => x.Status == Enum.StatusTicket.VENDIDO).Sum(i => i.Value)) / 100
                                    ),
                                TotalValue = i.Tickets.Where(x => x.Status == Enum.StatusTicket.VENDIDO).Sum(i => i.Value)
                            },
                            Remaining = new RemainingDto()
                            {
                                Percent = (i.Tickets.Count == 0) ? 100 :
                                    (
                                        Convert.ToDouble(i.Tickets.Count(x => x.Status == Enum.StatusTicket.DISPONIVEL))
                                        / Convert.ToDouble(i.Tickets.Count)
                                    ) * 100,
                                Amount = i.Tickets.Count(x => x.Status == Enum.StatusTicket.DISPONIVEL)
                            }
                        },
                    }
                );
            });
            return listLot;
        }
    }
}