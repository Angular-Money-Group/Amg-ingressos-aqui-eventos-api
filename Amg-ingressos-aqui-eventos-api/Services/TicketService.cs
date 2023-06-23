using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model.Querys;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class TicketService : ITicketService
    {
        private ITicketRepository _ticketRepository;
        private IVariantRepository _variantRepository;
        private MessageReturn _messageReturn;

        public TicketService(ITicketRepository ticketRepository, IVariantRepository variantRepository)
        {
            _ticketRepository = ticketRepository;
            _variantRepository = variantRepository;
            _messageReturn = new MessageReturn();
        }
        public async Task<MessageReturn> SaveAsync(Ticket ticket)
        {
            try
            {
                if (ticket.IdLot == null)
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

        public async Task<MessageReturn> DeleteAsync(List<string> tickets)
        {
            try
            {
                _messageReturn.Data = await _ticketRepository.DeleteMany<object>(tickets);
            }
            catch (SaveTicketException ex)
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

        public async Task<MessageReturn> DeleteTicketsByLot(string lotId)
        {
            try
            {
                _messageReturn.Data = await _ticketRepository.DeleteByLot<object>(lotId);
            }
            catch (SaveTicketException ex)
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
                _messageReturn.Data = result.Where(i => i.IdUser == null).ToList();

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

        public async Task<MessageReturn> GetTicketByIdDataUser(string id)
        {
            try
            {
                id.ValidateIdMongo();
                var ticketUserData = (GetTicketDataUser)_ticketRepository
                                                .GetTicketByIdDataUser<GetTicketDataUser>(id).Result;

                var variantData = _variantRepository.FindById<List<Model.Variant>>(ticketUserData.Lot.FirstOrDefault().IdVariant).Result;

                _messageReturn.Data = new TicketUserDto()
                {
                    Id= ticketUserData._id,
                    IdLot= ticketUserData.IdLot,
                    IdUser= ticketUserData.IdUser,
                    Status = ticketUserData.Status,
                    QrCode = ticketUserData.QrCode,
                    Value = ticketUserData.Value,
                    Variante = variantData.FirstOrDefault().Name,

                    User = new UserDto(){
                        _id = ticketUserData.User.FirstOrDefault()._id,
                        cpf = ticketUserData.User.FirstOrDefault().cpf,
                        email=ticketUserData.User.FirstOrDefault().email,
                        name=ticketUserData.User.FirstOrDefault().name
                    },
                };

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

        public async Task<MessageReturn> UpdateTicketsAsync(string id, Ticket ticket)
        {
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _ticketRepository.UpdateTicketsAsync<object>(id, ticket);
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