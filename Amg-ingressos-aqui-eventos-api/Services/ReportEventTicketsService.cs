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
        private MessageReturn _messageReturn = new ();
        private IEventRepository _eventRepository;

        public ReportEventTicketsService(IEventRepository eventRepository){
            _eventRepository= eventRepository;
        }
        public async Task<MessageReturn> ProcessReportEventTickets(string idOrganizer)
        {
            try
            {
                if (string.IsNullOrEmpty(idOrganizer))
                    throw new SaveTicketException("Id Organizador é Obrigatório.");

                idOrganizer.ValidateIdMongo();

                List<GetEventWitTickets> eventData = await _eventRepository.GetAllEventsWithTickets(string.Empty,idOrganizer);
                List<ReportEventOrganizerDetailDto> reports = new();
                eventData.ForEach(x=> {
                    reports.Add(ProcessEvent(x, string.Empty));
                });

                _messageReturn.Data = new ReportEventOrganizerDto()
                {
                    QuantidadeIngressos = reports.Sum(x=> x.QuantidadeIngressos),
                    Cortesias = new CortesiasReportDto(){
                        Entregues = new(){
                            Percent = reports.Sum(i=> i.Cortesias.Entregues.Percent),
                            Quantidade = reports.Sum(i=> i.Cortesias.Entregues.Quantidade),
                        },
                        Restantes= new(){
                            Percent = reports.Sum(i=> i.Cortesias.Restantes.Percent),
                            Quantidade = reports.Sum(i=> i.Cortesias.Restantes.Quantidade),
                            Taxa = 15, 
                            ValorReceber = reports.Sum(i=> i.Cortesias.Restantes.ValorReceber),
                            ValorTotal = reports.Sum(i=> i.Cortesias.Restantes.ValorTotal)
                        }
                    },
                    Ingressos = new IngressosReportDto(){
                        Restantes = new(){
                            Percent = (Convert.ToDouble(reports.Sum(i=> i.Ingressos.Restantes.Quantidade)) / Convert.ToDouble(reports.Sum(x=> x.QuantidadeIngressos))) * 100,
                            Quantidade = reports.Sum(i=> i.Ingressos.Restantes.Quantidade),
                            Taxa = reports.Sum(i=> i.Ingressos.Restantes.Taxa), 
                            ValorReceber = reports.Sum(i=> i.Ingressos.Restantes.ValorReceber),
                            ValorTotal = reports.Sum(i=> i.Ingressos.Restantes.ValorTotal)
                        },
                        Vendidos= new(){
                            Percent = (Convert.ToDouble(reports.Sum(i=> i.Ingressos.Vendidos.Quantidade)) / Convert.ToDouble(reports.Sum(x=> x.QuantidadeIngressos))) * 100,
                            Quantidade = reports.Sum(i=> i.Ingressos.Vendidos.Quantidade),
                            Taxa = reports.Sum(i=> i.Ingressos.Vendidos.Taxa), 
                            ValorReceber = reports.Sum(i=> i.Ingressos.Vendidos.ValorReceber),
                            ValorTotal = reports.Sum(i=> i.Ingressos.Vendidos.ValorTotal)
                        }

                    }
                };
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

            return _messageReturn;
        }
        public async Task<MessageReturn> ProcessReportEventTicketsDetail(string idEvent, string idVariant)
        {
            if (string.IsNullOrEmpty(idEvent))
                throw new SaveTicketException("Id Evento é Obrigatório.");
            idEvent.ValidateIdMongo();
            if (string.IsNullOrEmpty(idVariant))
                throw new SaveTicketException("Id Variante é Obrigatório.");
            idVariant.ValidateIdMongo();

                List<GetEventWitTickets> eventData = await _eventRepository.GetAllEventsWithTickets(idEvent,string.Empty);
                var eventDataProcess  =eventData.FirstOrDefault(i=> i._id==idEvent);
                _messageReturn.Data = ProcessEvent(eventDataProcess, idVariant);
                return _messageReturn;
        }
        private ReportEventOrganizerDetailDto ProcessEvent(GetEventWitTickets eventData, string? idVariant)
        {
            var variants = ProcessVariant(eventData, string.Empty);
            return new ReportEventOrganizerDetailDto()
            {
                Nome = eventData.Name,
                QuantidadeIngressos = variants.Sum(i => i.QuantidadeIngressos),
                Ingressos = new IngressosReportDto()
                {
                    Vendidos = new VendidosDto()
                    {
                        Percent = (Convert.ToDouble(variants.Sum(i => i.Ingressos.Vendidos.Quantidade)) / Convert.ToDouble(variants.Sum(i => i.QuantidadeIngressos))) * 100,
                        Quantidade = variants.Sum(i => i.Ingressos.Vendidos.Quantidade),
                        Taxa = 15,
                        ValorReceber = variants.Sum(i => i.Ingressos.Vendidos.ValorReceber),
                        ValorTotal = variants.Sum(i => i.Ingressos.Vendidos.ValorTotal)
                    },
                    Restantes = new RestantesDto()
                    {
                        Percent = (Convert.ToDouble(variants.Sum(i => i.Ingressos.Restantes.Quantidade)) / Convert.ToDouble(variants.Sum(i => i.QuantidadeIngressos))) * 100,
                        Quantidade = variants.Sum(i => i.Ingressos.Restantes.Quantidade),
                        Taxa = 15,
                        ValorReceber = variants.Sum(i => i.Ingressos.Restantes.ValorReceber),
                        ValorTotal = variants.Sum(i => i.Ingressos.Restantes.ValorTotal)
                    }
                }
                         ,
                Cortesias = new CortesiasReportDto()
                {
                    Entregues = new EntreguesReportDto()
                    {
                        Percent = variants.Sum(i => i.Cortesias.Entregues.Percent),
                        Quantidade = variants.Sum(i => i.Cortesias.Entregues.Quantidade)
                    },
                    Restantes = new RestantesDto()
                    {
                        Percent = variants.Sum(i => i.Cortesias.Restantes.Percent),
                        Quantidade = variants.Sum(i => i.Cortesias.Restantes.Quantidade)
                    }
                }
                         ,
                Variant = string.IsNullOrEmpty(idVariant)? null: variants.FirstOrDefault(
                    x => x.Nome == eventData.Variant.FirstOrDefault(
                        i => i._id == idVariant).Name)
            };
        }
        private List<VariantReportDto> ProcessVariant(GetEventWitTickets eventData, string idVariant)
        {
            List<VariantReportDto> variantList = new List<VariantReportDto>();
            var variant = string.IsNullOrEmpty(idVariant) ? eventData.Variant : eventData.Variant.Where(i => i._id == idVariant).ToList();
            variant.ForEach(x =>
            {
                var lots = ProcessLotes(x);
                var variantDto = new VariantReportDto()
                {
                    Nome = x.Name,
                    QuantidadeIngressos = lots.Sum(i => i.QuantidadeIngressos),
                    Ingressos = new IngressosReportDto()
                    {
                        Vendidos = new VendidosDto()
                        {
                            Percent = (Convert.ToDouble(lots.Sum(i => i.Ingressos.Vendidos.Quantidade)) / Convert.ToDouble(lots.Sum(i => i.QuantidadeIngressos))) * 100,
                            Quantidade = lots.Sum(i => i.Ingressos.Vendidos.Quantidade),
                            Taxa = 15,
                            ValorReceber = lots.Sum(i => i.Ingressos.Vendidos.ValorReceber),
                            ValorTotal = lots.Sum(i => i.Ingressos.Vendidos.ValorTotal)
                        },
                        Restantes = new RestantesDto()
                        {
                            Percent = (Convert.ToDouble(lots.Sum(i => i.Ingressos.Restantes.Quantidade)) / Convert.ToDouble(lots.Sum(i => i.QuantidadeIngressos))) * 100,
                            Quantidade = lots.Sum(i => i.Ingressos.Restantes.Quantidade),
                            Taxa = 15,
                            ValorReceber = lots.Sum(i => i.Ingressos.Restantes.ValorReceber),
                            ValorTotal = lots.Sum(i => i.Ingressos.Restantes.ValorTotal)
                        }
                    }
                         ,
                    Cortesias = ProcessCortesy(eventData, x)
                         ,
                    Lotes = lots
                };
                variantList.Add(variantDto);
            });
            return variantList;
        }
        private CortesiasReportDto ProcessCortesy(GetEventWitTickets eventData, Model.Querys.GetEventwithTicket.Variant x)
        {
            var cortesyHistory = eventData.Courtesy.CourtesyHistory.Where(i => i.Variant == x.Name).ToList();
            var cortesyRemaining = eventData.Courtesy.RemainingCourtesy.Where(i => i.VariantId == x._id).ToList();
            return new CortesiasReportDto()
            {
                Entregues = cortesyRemaining.Sum(x => x.Quantity) !=0 ?
                new EntreguesReportDto()
                {
                    Percent = (Convert.ToDouble(cortesyHistory.Sum(x => x.Quantity) / Convert.ToDouble(cortesyRemaining.Sum(x => x.Quantity)))) * 100,
                    Quantidade = cortesyHistory.Sum(x => x.Quantity)
                }:
                new EntreguesReportDto()
                {
                    Percent = 100,
                    Quantidade = cortesyHistory.Sum(x => x.Quantity)
                },
                Restantes = cortesyRemaining.Sum(x => x.Quantity) !=0 ?
                new RestantesDto()
                {
                    Percent = (Convert.ToDouble((cortesyRemaining.Sum(x => x.Quantity) - cortesyHistory.Sum(x => x.Quantity))) / Convert.ToDouble(cortesyRemaining.Sum(x => x.Quantity))) * 100,
                    Quantidade = (cortesyRemaining.Sum(x => x.Quantity) - cortesyHistory.Sum(x => x.Quantity))
                } :
                new RestantesDto()
                {
                    Percent = 0,
                    Quantidade = cortesyRemaining.Sum(x => x.Quantity)
                }

            };
        }
        private List<LoteReportDto> ProcessLotes(Model.Querys.GetEventwithTicket.Variant? variant)
        {
            var listLot = new List<LoteReportDto>();
            variant?.Lot.ForEach(i =>
            {
                listLot.Add(
                    new LoteReportDto()
                    {
                        Nome = i.Identificate.ToString(),
                        QuantidadeIngressos = i.ticket.Count(),
                        Ingressos = new IngressosReportDto()
                        {
                            Vendidos = new VendidosDto()
                            {
                                Percent = (Convert.ToDouble(i.ticket.Count(x => x.isSold)) / Convert.ToDouble(i.ticket.Count())) * 100,
                                Quantidade = i.ticket.Count(x => x.isSold),
                                Taxa = 15,
                                ValorReceber = i.ticket.Where(x => x.isSold).Sum(i => i.Value) - ((15 * i.ticket.Where(x => x.isSold).Sum(i => i.Value)) / 100),
                                ValorTotal = i.ticket.Where(x => x.isSold).Sum(i => i.Value)
                            },
                            Restantes = new RestantesDto()
                            {
                                Percent = (Convert.ToDouble(i.ticket.Count(x => !x.isSold)) / Convert.ToDouble(i.ticket.Count())) * 100,
                                Quantidade = i.ticket.Count(x => !x.isSold)
                            }
                        }
                    ,
                    }
                );
            });
            return listLot;
        }
    }
}