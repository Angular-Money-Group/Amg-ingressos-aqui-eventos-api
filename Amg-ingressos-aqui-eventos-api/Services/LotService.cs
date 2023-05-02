using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using System.Text;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;

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
                    await _ticketService.SaveAsync(new Ticket()
                    {
                        //IdLot = _messageReturn.Data.ToString(),
                        Value = lot.ValueTotal / lot.TotalTickets
                    });
                }
            }
            //catch (SaveTicketException ex)
            //{
            //    DeleteAsync(lot.Id);
            //    _messageReturn.Message = ex.Message;
           // }
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
        private async Task DeleteAsync(string id)
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