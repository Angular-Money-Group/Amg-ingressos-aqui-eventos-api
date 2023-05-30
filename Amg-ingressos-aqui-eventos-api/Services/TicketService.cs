using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
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
                if(ticket.IdLot== null)
                    throw new SaveTicketException("Id Lote é Obrigatório.");
                if (ticket?.Value == 0)
                    throw new SaveTicketException("Valor do Ingresso é Obrigatório.");

                ticket?.IdLot?.ValidateIdMongo();

                _messageReturn.Data = await _ticketRepository.Save<object>(ticket!);
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

        public async Task<MessageReturn> GetTicketByUser(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();
                var ticket = new Ticket() { IdUser = idUser };
                _messageReturn.Data = await _ticketRepository.GetTickets<List<Ticket>>(ticket);
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

        public async Task<MessageReturn> GetTicketsByLot(string idLot)
        {
            try
            {
                idLot.ValidateIdMongo();
                var ticket = new Ticket() { IdLot = idLot };
                _messageReturn.Data = await _ticketRepository.GetTickets<List<Ticket>>(ticket);
            }
            catch (GetRemeaningTicketsExepition ex)
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

        public async Task<MessageReturn> GetTicketsRemainingByLot(string idLot)
        {
            try
            {
                idLot.ValidateIdMongo();
                var ticket = new Ticket() { IdLot = idLot };
                var result = await _ticketRepository.GetTickets<List<Ticket>>(ticket);
                _messageReturn.Data = result.Where(i=> i.IdUser == null).ToList();

            }
            catch (GetRemeaningTicketsExepition ex)
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

        public async Task<MessageReturn> GetTicketById(string id)
        {
            try
            {
                id.ValidateIdMongo();
                var ticket = new Ticket() { Id = id };
                _messageReturn.Data = (Ticket)_ticketRepository.GetTickets<List<Ticket>>(ticket).Result.FirstOrDefault();

            }
            catch (GetRemeaningTicketsExepition ex)
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

        public async Task<MessageReturn> UpdateTicketsAsync(string id, Ticket ticket){
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _ticketRepository.UpdateTicketsAsync<object>(id,ticket);
            }
            catch (NotModificateTicketsExeption ex)
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
    }
}