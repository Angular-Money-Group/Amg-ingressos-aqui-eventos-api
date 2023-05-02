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
        private ITicketRepository _ticketRepository;
        private MessageReturn _messageReturn;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
            _messageReturn = new MessageReturn();
        }
        public async Task<MessageReturn> SaveAsync(Ticket ticket)
        {
            try
            {
                ticket?.IdLot?.ValidateIdMongo();
                if (ticket?.Value == 0)
                    throw new SaveTicketException("Valor do Ingresso é Obrigatório.");

                _messageReturn.Data = await _ticketRepository.Save<object>(ticket);
            }
            catch (SaveTicketException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetUserTicketsAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _ticketRepository.GetUserTickets<List<Ticket>>(id);
            }
            catch (SaveTicketException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (IdMongoException ex)
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