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
    public class TicketService : ITicketService
    {
        private IVariantRepository _ticketRepository;
        private MessageReturn _messageReturn;

        public TicketService(IVariantRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
            _messageReturn = new MessageReturn();
        }
        public async Task<MessageReturn> SaveAsync(Ticket ticket)
        {
            try
            {
                ticket.IdLote.ValidateIdMongo();

                _messageReturn.Data = await _ticketRepository.Save<object>(ticket);
            }
            catch (SaveVariantException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
    }
}