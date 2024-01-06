using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class LotService : ILotService
    {
        private readonly ILotRepository _lotRepository;
        private readonly ITicketService _ticketService;
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<LotService> _logger;
        private readonly IVariantRepository _variantRepository;

        public LotService(
            ILotRepository lotRepository,
            ITicketService ticketService,
            IVariantRepository variantRepository,
            ILogger<LotService> logger)
        {
            _lotRepository = lotRepository;
            _ticketService = ticketService;
            _variantRepository = variantRepository;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(LotWithTicketDto lot)
        {
            try
            {
                ValidateModelSave(lot);
                lot.Status = Enum.EnumStatusLot.Open;
                var lotModel = new LotWithTicketDto().DtoToModel(lot);
                await _lotRepository.Save(lotModel);
                List<Ticket> listTicket = new List<Ticket>();
                for (int i = 0; i < lot.TotalTickets; i++)
                {
                    listTicket.Add(new Ticket()
                    {
                        ReqDocs = lot.ReqDocs,
                        IdLot = lotModel?.Id ?? string.Empty,
                        Value = lot.ValueTotal,
                        TicketCortesia = false
                    });
                }

                _ = _ticketService.SaveManyAsync(listTicket);
                _messageReturn.Data = lotModel;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Lote"), lot);
                _ = DeleteAsync(lot.Id ?? string.Empty);
                throw;
            }
        }

        public async Task<MessageReturn> SaveManyAsync(List<LotWithTicketDto> listLot)
        {
            try
            {
                listLot.ForEach(i => { ValidateModelSave(i); });
                var listLotModel = new LotWithTicketDto().ListDtoToListModel(listLot);
                _messageReturn.Data = await _lotRepository.SaveMany(listLotModel);
                listLot.ForEach(x =>
                {
                    List<Ticket> listTicket = new List<Ticket>();
                    for (int i = 0; i < x.TotalTickets; i++)
                    {
                        listTicket.Add(new Ticket()
                        {
                            ReqDocs = x.ReqDocs,
                            IdLot = x.Id ?? string.Empty,
                            Value = x.ValueTotal
                        });
                    }
                    _ticketService.SaveManyAsync(listTicket);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveManyAsync), "Lotes"), listLot);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                await _ticketService.DeleteTicketsByLot(id);
                _messageReturn.Data = await _lotRepository.Delete(id);
            }
            catch (DeleteException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Lote"), id);
                _ = DeleteAsync(id ?? string.Empty);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Lote"), id);
                _ = DeleteAsync(id ?? string.Empty);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteByVariantAsync(string idVariant)
        {
            try
            {
                idVariant.ValidateIdMongo();
                _messageReturn.Data = await _lotRepository.DeleteByVariant(idVariant);
            }
            catch (DeleteException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteByVariantAsync), "Lote"), idVariant);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteByVariantAsync), "Lote"), idVariant);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> EditAsync(string id, Lot lot)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _lotRepository.Edit(id, lot);
                await _ticketService.DeleteTicketsByLot(id);

                for (int i = 0; i < lot.TotalTickets; i++)
                {
                    await _ticketService.SaveAsync(new Ticket()
                    {
                        IdLot = id,
                        Value = lot.ValueTotal
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), "Lote"), lot);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteManyAsync(List<string> listLot)
        {
            try
            {
                listLot.ForEach(async lotId =>
                {
                    await _ticketService.DeleteTicketsByLot(lotId);
                });

                _messageReturn.Data = await _lotRepository.DeleteMany<object>(listLot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteManyAsync), "Lote"), listLot);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetByIdVariant(string idVariant)
        {
            try
            {
                idVariant.ValidateIdMongo();
                _messageReturn.Data = await _lotRepository.GetLotByIdVariant<Lot>(idVariant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByIdVariant), "Lote"), idVariant);
                throw;
            }

            return _messageReturn;
        }

        private void ValidateModelSave(Lot lot)
        {
            if (lot.Identificate == 0)
                throw new SaveException("Identificaror é Obrigatório.");
            else if (lot.StartDateSales == DateTime.MinValue || lot.StartDateSales == DateTime.MaxValue)
                throw new SaveException("Data Inicio de venda é Obrigatório.");
            else if (lot.EndDateSales == DateTime.MinValue || lot.EndDateSales == DateTime.MaxValue)
                throw new SaveException("Data final de venda é Obrigatório.");
        }

        public async Task<MessageReturn> ManagerLotsAsync(string idLote, DateTime dateManagerLots)
        {
            try
            {
                //Consultar os lotes que encerram na data de gestão de lotes
                var lots = await _lotRepository.GetLotByEndDateSales(dateManagerLots);

                if (lots != null && lots.Count() > 0)
                {
                    //Percorrer os lotes e executar a gestao
                    foreach (Lot lot in lots)
                    {
                        //Consultar o numero de ingressos(tickets) não vendidos do lote
                        long qtdTicketsNaoUsados = await _ticketService.GetCountTicketsNoUser(lot.Id);

                        if (qtdTicketsNaoUsados > 0)
                        {
                            //Consultar se existe mais um lote para o evento - 

                            //Se existir mais lotes - Os ingressos não vendidos (quantidade), inserir eles no lote que vai iniciar

                            //Finalizar o lote (atualizar o status)
                            _lotRepository.Edit(idLote, new Lot() { Status = Enum.EnumStatusLot.Finished });

                            //Não existe mais lote - Finalizar a variação (atualizar o status)
                            _variantRepository.Edit(lot.IdVariant, new Variant() { Status = Enum.EnumStatusVariant.Finished });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByIdVariant), "dateManagerLots"), dateManagerLots);
                throw;
            }
            return _messageReturn;
        }
    }
}