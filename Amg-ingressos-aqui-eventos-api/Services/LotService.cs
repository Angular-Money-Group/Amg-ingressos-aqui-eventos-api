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
                    await _ticketService.SaveAsync(new Ticket()
                    {
                        IdLot = _messageReturn.Data.ToString(),
                        Value = lot.ValueTotal / lot.TotalTickets
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

        public async Task<MessageReturn> VerifyLotsAvaibleAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();

                var lotsStatus = new List<LotAvailable>();

                List<Lot> pLotsByVariant = await _lotRepository.FindByIdVariant<List<Lot>>(id);

                if (pLotsByVariant.Count == 0)
                {
                    throw new FindLotsByIdVariantException("Variantes não encrontradas");
                }

                for (int i = 0; i < pLotsByVariant.Count; i++)
                {
                    lotsStatus.Add(await CheckTicketAvailability(pLotsByVariant[i]));
                };

                _messageReturn.Data = lotsStatus;

            }
            catch (FindLotsByIdVariantException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _messageReturn.Message = ex.Message;
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
            if (lot.Identificate == 0)
                throw new SaveLotException("Identificaror é Obrigatório.");
            else if (lot.StartDateSales == DateTime.MinValue || lot.StartDateSales == DateTime.MaxValue)
                throw new SaveLotException("Data Inicio de venda é Obrigatório.");
            else if (lot.EndDateSales == DateTime.MinValue || lot.EndDateSales == DateTime.MaxValue)
                throw new SaveLotException("Data final de venda é Obrigatório.");
        }

        private async Task<LotAvailable> CheckTicketAvailability(Lot lot)
        {
            try
            {
                var pResult = await _ticketRepository.GetTicketsRemaining<List<Lot>>(lot.Id);

                DateTime currentDate = DateTime.Now;
                DateTime endDateSales = lot.EndDateSales;

                if (pResult.Count == 0)
                {
                    return new LotAvailable()
                    {
                        Idlot = lot.Id,
                        Available = false
                    };
                }
                else if (endDateSales <= currentDate)
                {
                    return new LotAvailable()
                    {
                        Idlot = lot.Id,
                        Available = false
                    };
                }
                else
                {
                    return new LotAvailable()
                    {
                        Idlot = lot.Id,
                        Available = true
                    };
                }
            }
            catch (GetRemeaningTicketsExepition ex)
            {
                return new LotAvailable()
                {
                    Idlot = lot.Id,
                    Available = false
                };
            }
            catch (Exception ex)
            {
                return new LotAvailable()
                {
                    Idlot = lot.Id,
                    Available = false
                };
            }
        }
    }
}