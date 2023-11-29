using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto.report;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Utils;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class ReportEventTicketsService : IReportEventTickets
    {
        private MessageReturn _messageReturn = new();
        private IEventRepository _eventRepository;
        private ILogger<ReportEventTicketsService> _logger;

        public ReportEventTicketsService(
            IEventRepository eventRepository, 
            ILogger<ReportEventTicketsService> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task<MessageReturn> ProcessReportEventTickets(string idOrganizer)
        {
            try
            {
                if (string.IsNullOrEmpty(idOrganizer))
                    throw new ReportException("Id Organizador é Obrigatório.");

                idOrganizer.ValidateIdMongo();

                List<GetEventWitTickets> eventData = await _eventRepository.GetAllEventsWithTickets(
                    string.Empty,
                    idOrganizer
                );
                List<ReportEventOrganizerDetailDto> reports = new();
                eventData.ForEach(x =>
                {
                    reports.Add(ProcessEvent(x, string.Empty));
                });

                _messageReturn.Data = new ReportEventOrganizerDto()
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTickets), "Eventos por ticket"), idOrganizer);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTickets), "Eventos por ticket"), idOrganizer);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTickets), "Eventos por ticket"), idOrganizer);
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

                List<GetEventWitTickets> eventData = await _eventRepository.GetAllEventsWithTickets(idEvent, string.Empty);
                var eventDataProcess = eventData.FirstOrDefault(i => i._id == idEvent) ?? throw new ReportException("Dados não pode ser null");
                _messageReturn.Data = ProcessEvent(eventDataProcess, idVariant);
                return _messageReturn;
            }
            catch (ReportException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetail), "Eventos por ticket"));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetail), "Eventos por ticket"));
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
                List<GetEventWitTickets> eventData = await _eventRepository.GetAllEventsWithTickets(idEvent, string.Empty);
                var eventDataProcess = eventData.FirstOrDefault(i => i._id == idEvent) ?? throw new ReportException("Dados não pode ser null");
                _messageReturn.Data = ProcessEvent(eventDataProcess, string.Empty);
                return _messageReturn;
            }
            catch (ReportException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetails), "Eventos por ticket"), idEvent);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Report, this.GetType().Name, nameof(ProcessReportEventTicketsDetails), "Eventos por ticket"), idEvent);
                throw;
            }
        }

        private ReportEventOrganizerDetailDto ProcessEvent(GetEventWitTickets eventData, string? idVariant)
        {
            var variants = ProcessVariant(eventData, string.Empty);
            return new ReportEventOrganizerDetailDto()
            {
                Name = eventData.Name,
                AmountTicket = variants.Sum(i => i.AmountTickets),
                Tickets = new TicketsReportDto()
                {
                    Sold = new SoldDto()
                    {
                        Percent =
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
                        Percent =
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
                    : variants?.FirstOrDefault(x => x.Name == eventData?.Variant?.FirstOrDefault(i => i._id == idVariant)?.Name) ?? new VariantReportDto()
            };
        }

        private CourtesyReportDto VerifyCortesy(List<VariantReportDto> variants, GetEventWitTickets eventData)
        {
            if (eventData.Courtesy.RemainingCourtesy.Count == 0) return new CourtesyReportDto();
            return new CourtesyReportDto()
            {
                Entregues = new DeliveredReportDto()
                {
                    Percent =
                            (
                                Convert.ToDouble(
                                    variants.Sum(i => i.Cortesys.Entregues.Amount)
                                ) / Convert.ToDouble(variants.Sum(i => i.Cortesys.Restantes.Amount))
                            ) * 100,
                    Amount = variants.Sum(i => i.Cortesys.Entregues.Amount)
                },
                Restantes = new RemainingDto()
                {
                    Percent =
                            (
                                Convert.ToDouble(
                                    variants.Sum(i => i.Cortesys.Entregues.Amount)
                                ) / Convert.ToDouble(variants.Sum(i => i.Cortesys.Restantes.Amount))
                            ) * 100,
                    Amount = variants.Sum(i => i.Cortesys.Restantes.Amount)
                }
            };
        }

        private List<VariantReportDto> ProcessVariant(GetEventWitTickets eventData, string idVariant)
        {
            List<VariantReportDto> variantList = new List<VariantReportDto>();
            var variant = string.IsNullOrEmpty(idVariant)
                ? eventData.Variant
                : eventData.Variant.Where(i => i._id == idVariant).ToList();
            variant.ForEach(x =>
            {
                var lots = ProcessLotes(x);
                var variantDto = new VariantReportDto()
                {
                    Name = x.Name,
                    AmountTickets = lots.Sum(i => i.AmountTicket),
                    Tickets = new TicketsReportDto()
                    {
                        Sold = new SoldDto()
                        {
                            Percent =
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
                            Percent =
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
                    Cortesys = ProcessCortesy(eventData, x),
                    Lots = lots
                };
                variantList.Add(variantDto);
            });
            return variantList;
        }

        private CourtesyReportDto ProcessCortesy(
            GetEventWitTickets eventData,
            Model.Querys.GetEventwithTicket.Variant x
        )
        {
            var cortesyHistory = eventData.Courtesy.CourtesyHistory
                .Where(i => i.Variant == x.Name)
                .ToList();
            var cortesyRemaining = eventData.Courtesy.RemainingCourtesy
                .Where(i => i.VariantId == x._id)
                .ToList();
            return new CourtesyReportDto()
            {
                Entregues =
                    cortesyRemaining.Sum(x => x.Quantity) != 0
                        ? new DeliveredReportDto()
                        {
                            Percent =
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
                            Percent =
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

        private List<LotReportDto> ProcessLotes(Model.Querys.GetEventwithTicket.Variant? variant)
        {
            var listLot = new List<LotReportDto>();
            variant?.Lot.ForEach(i =>
            {
                listLot.Add(
                    new LotReportDto()
                    {
                        Name = i.Identificate.ToString(),
                        AmountTicket = i.ticket.Count(),
                        Tickets = new TicketsReportDto()
                        {
                            Sold = new SoldDto()
                            {
                                Percent =
                                    (
                                        Convert.ToDouble(i.ticket.Count(x => x.IsSold))
                                        / Convert.ToDouble(i.ticket.Count())
                                    ) * 100,
                                Amount = i.ticket.Count(x => x.IsSold),
                                Tax = 15,
                                ReceiveValue =
                                    i.ticket.Where(x => x.IsSold).Sum(i => i.Value)
                                    - (
                                        (15 * i.ticket.Where(x => x.IsSold).Sum(i => i.Value)) / 100
                                    ),
                                TotalValue = i.ticket.Where(x => x.IsSold).Sum(i => i.Value)
                            },
                            Remaining = new RemainingDto()
                            {
                                Percent =
                                    (
                                        Convert.ToDouble(i.ticket.Count(x => !x.IsSold))
                                        / Convert.ToDouble(i.ticket.Count())
                                    ) * 100,
                                Amount = i.ticket.Count(x => !x.IsSold)
                            }
                        },
                    }
                );
            });
            return listLot;
        }
    }
}