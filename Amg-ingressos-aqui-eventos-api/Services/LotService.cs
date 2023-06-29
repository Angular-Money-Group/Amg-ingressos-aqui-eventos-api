using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class LotService : ILotService
    {
        private ILotRepository _lotRepository;
        private ITicketRepository _ticketRepository;
        private ITicketService _ticketService;
        private MessageReturn _messageReturn;

        public LotService(ILotRepository lotRepository, ITicketService ticketService, ITicketRepository ticketRepository)
        {
            _lotRepository = lotRepository;
            _ticketRepository = ticketRepository;
            _ticketService = ticketService;
            _messageReturn = new MessageReturn();
        }
        public async Task<MessageReturn> SaveAsync(Lot lot)
        {
            try
            {
                ValidateModelSave(lot);
                lot.Status = Enum.StatusLot.Open;
                _messageReturn.Data = await _lotRepository.Save<object>(lot);
                for (int i = 0; i < lot.TotalTickets; i++)
                {
                    _ticketService.SaveAsync(new Ticket()
                    {
                        ReqDocs = lot.ReqDocs,
                        IdLot = _messageReturn.Data.ToString(),
                        Value = lot.ValueTotal
                    });
                }
            }
            catch (SaveTicketException ex)
            {
                DeleteAsync(lot.Id);
                _messageReturn.Message = ex.Message;
            }
            catch (SaveLotException ex)
            {
                DeleteAsync(lot.Id);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                DeleteAsync(lot.Id);
                throw ex;
            }

            return _messageReturn;
        }
        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _lotRepository.Delete<object>(id);
            }
            catch (SaveLotException ex)
            {
                DeleteAsync(id);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                DeleteAsync(id);
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> EditAsync(string id, Lot lotEdit)
        {
            try
            {
                _messageReturn.Data = await _lotRepository.Edit<object>(id, lotEdit);

                await _ticketService.DeleteTicketsByLot(id);

                for (int i = 0; i < lotEdit.TotalTickets; i++)
                {
                    await _ticketService.SaveAsync(new Ticket()
                    {
                        IdLot = id,
                        Value = lotEdit.ValueTotal / lotEdit.TotalTickets
                    });
                }

            }
            catch (SaveLotException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteManyAsync(List<string> Lot)
        {
            try
            {
                Lot.ForEach(async lotId =>
                {
                    await _ticketService.DeleteTicketsByLot(lotId);
                });

                _messageReturn.Data = await _lotRepository.DeleteMany<object>(Lot);

            }
            catch (SaveLotException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        private void ValidateModelSave(Lot lot)
        {
            if (lot.Identificate == 0)
                throw new SaveLotException("Identificaror é Obrigatório.");
            else if (lot.StartDateSales == DateTime.MinValue || lot.StartDateSales == DateTime.MaxValue)
                throw new SaveLotException("Data Inicio de venda é Obrigatório.");
            else if (lot.EndDateSales == DateTime.MinValue || lot.EndDateSales == DateTime.MaxValue)
                throw new SaveLotException("Data final de venda é Obrigatório.");
        }
    }
}