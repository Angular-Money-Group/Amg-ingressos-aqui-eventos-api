using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using System.Text;
using Amg_ingressos_aqui_eventos_api.Exceptions;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class LotService : ILotService
    {
        private ILotRepository _lotRepository;
        private ITicketService _ticketService;
        private MessageReturn _messageReturn;

        public LotService(ILotRepository lotRepository, ITicketService ticketService)
        {
            _lotRepository = lotRepository;
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
                    Task<MessageReturn> task = _ticketService.SaveAsync(new Ticket()
                    {
                        IdLote = _messageReturn.Data.ToString()
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

        private void ValidateModelSave(Lot lot)
        {
            if (lot.Description == "")
                throw new SaveLotException("Descrição é Obrigatório.");
            else if (lot.StartDateSales == DateTime.MinValue || lot.StartDateSales == DateTime.MaxValue)
                throw new SaveLotException("Data Inicio de venda é Obrigatório.");
            else if (lot.EndDateSales == DateTime.MinValue || lot.EndDateSales == DateTime.MaxValue)
                throw new SaveLotException("Data final de venda é Obrigatório.");
        }
    }
}