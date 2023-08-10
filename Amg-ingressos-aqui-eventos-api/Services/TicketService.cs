using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Infra;
using System.Text.Json;
using MongoDB.Bson;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class TicketService : ITicketService, IDisposable
    {
        private ITicketRepository _ticketRepository;
        private ITicketRowRepository _ticketRowRepository;
        private IVariantRepository _variantRepository;
        private ILotRepository _lotRepository;
        private IEmailService _emailService;
        private IEventRepository _eventRepository;
        private MessageReturn _messageReturn;
        private HttpClient _httpClient;

        public TicketService(
            ITicketRepository ticketRepository,
            ITicketRowRepository ticketRowRepository,
            IVariantRepository variantRepository,
            ICieloClient cieloClient,
            ILotRepository lotRepository,
            IEmailService emailService,
            IEventRepository eventRepository
        )
        {
            _ticketRepository = ticketRepository;
            _ticketRowRepository = ticketRowRepository;
            _variantRepository = variantRepository;
            _lotRepository = lotRepository;
            _emailService = emailService;
            _eventRepository = eventRepository;
            _httpClient = cieloClient.CreateClient();
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

        public async Task<MessageReturn> SaveManyAsync(List<Ticket> lstTicket)
        {
            try
            {
                _messageReturn.Data = await _ticketRepository.SaveMany(lstTicket);
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

        public async Task<MessageReturn> SendCourtesyTickets(
            GenerateCourtesyTicketDto CourtesyTicket
        )
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var ticketsRow = new StatusTicketsRow()
                {
                    Email = CourtesyTicket.Email,
                    TotalTickets = CourtesyTicket.quantity,
                    TicketStatus = new List<TicketStatusResult>(),
                };

                var rowId = _ticketRowRepository.SaveRowAsync<StatusTicketsRow>(ticketsRow).Result;

                await ProcessEmailSending(CourtesyTicket, rowId, ticketsRow);

                _messageReturn.Data = "Enviando...";
            }
            catch (Exception ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }

            return _messageReturn;
        }

        public async Task ProcessEmailSending(
            GenerateCourtesyTicketDto CourtesyTicket,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            for (var i = 0; i < CourtesyTicket.quantity; i++)
            {
                var ticketStatusResult = new TicketStatusResult()
                {
                    Identificate = i,
                    Status = TicketStatusEnum.Processando
                };

                ticketsRow.TicketStatus.Add(ticketStatusResult);
            }

            await _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);

            Model.Lot lotToGenerateTicket = _lotRepository
                .GetLotByIdVariant<Model.Lot>(CourtesyTicket.IdVariant)
                .Result;

            Model.Variant variantToGenerateTicket = _variantRepository
                .FindById<Model.Variant>(CourtesyTicket.IdVariant)
                .Result.FirstOrDefault();

            Model.Event eventToGenerateTicket = _eventRepository
                .FindByIdVariant<Model.Event>(variantToGenerateTicket.IdEvent)
                .Result;

            var idUser = new ObjectId().ToString();

            var isEmailSend = false;

            for (var i = 0; i < CourtesyTicket.quantity; i++)
            {
                try
                {
                    var ticketToSend = new Ticket()
                    {
                        IdLot = lotToGenerateTicket.Id,
                        IdUser = idUser,
                        Value = lotToGenerateTicket.ValueTotal,
                        isSold = true,
                        ReqDocs = false,
                    };

                    var IdTicket = SaveAsync(ticketToSend).Result.Data.ToString();

                    ticketsRow.TicketStatus[i].TicketId = IdTicket;

                    _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);

                    var nameImagem = GenerateQrCode(IdTicket).Result;

                    ticketToSend.Id = IdTicket;
                    ticketToSend.QrCode = "https://api.ingressosaqui.com/imagens/" + nameImagem;

                    UpdateTicketsAsync(IdTicket, ticketToSend);

                    var ticketEventDto = new TicketEventDataDto()
                    {
                        lot = new Dto.Lot()
                        {
                            _id = lotToGenerateTicket.Id,
                            identificate = lotToGenerateTicket.Identificate,
                            totalTickets = lotToGenerateTicket.TotalTickets,
                            valueTotal = lotToGenerateTicket.ValueTotal,
                            startDateSales = lotToGenerateTicket.StartDateSales,
                            endDateSales = lotToGenerateTicket.EndDateSales,
                            status = lotToGenerateTicket.Status,
                            reqDocs = lotToGenerateTicket.ReqDocs,
                            idVariant = lotToGenerateTicket.IdVariant
                        },
                        variant = new Dto.Variant()
                        {
                            _id = variantToGenerateTicket.Id,
                            name = variantToGenerateTicket.Name,
                            description = variantToGenerateTicket.Description,
                            hasPositions = variantToGenerateTicket.HasPositions,
                            status = variantToGenerateTicket.Status,
                            idEvent = variantToGenerateTicket.IdEvent,
                            reqDocs = variantToGenerateTicket.ReqDocs,
                            positions = variantToGenerateTicket.Positions
                        },
                        @event = new Dto.Event()
                        {
                            _id = eventToGenerateTicket._Id,
                            name = eventToGenerateTicket.Name,
                            local = eventToGenerateTicket.Local,
                            type = eventToGenerateTicket.Type,
                            image = eventToGenerateTicket.Image,
                            description = eventToGenerateTicket.Description,
                            startDate = eventToGenerateTicket.StartDate,
                            endDate = eventToGenerateTicket.EndDate,
                            status = eventToGenerateTicket.Status,
                            address = new Dto.Address()
                            {
                                cep = eventToGenerateTicket.Address.Cep,
                                addressDescription = eventToGenerateTicket
                                    .Address
                                    .AddressDescription,
                                number = eventToGenerateTicket.Address.Number,
                                neighborhood = eventToGenerateTicket.Address.Neighborhood,
                                complement = eventToGenerateTicket.Address.Complement,
                                referencePoint = eventToGenerateTicket.Address.ReferencePoint,
                                city = eventToGenerateTicket.Address.City,
                                state = eventToGenerateTicket.Address.State
                            },
                            idMeansReceipt = eventToGenerateTicket.IdMeansReceipt,
                            idOrganizer = eventToGenerateTicket.IdOrganizer,
                            highlighted = eventToGenerateTicket.Highlighted
                        },
                        id = variantToGenerateTicket.Id,
                        idLot = lotToGenerateTicket.Id,
                        idUser = idUser,
                        position = null,
                        value = lotToGenerateTicket.ValueTotal,
                        isSold = true,
                        reqDocs = false,
                        qrCode = "https://api.ingressosaqui.com/imagens/" + nameImagem
                    };

                    isEmailSend = await ProcessEmail(
                        CourtesyTicket.Email,
                        ticketEventDto,
                        ticketToSend.QrCode,
                        rowId,
                        ticketsRow,
                        i
                    );
                }
                catch (Exception ex)
                {
                    ticketsRow.TicketStatus[i].Status = TicketStatusEnum.Erro;
                    ticketsRow.TicketStatus[i].Message = ex.Message;

                    _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
                }
            }

            var editEvent = eventToGenerateTicket;

            editEvent.Courtesy.CourtesyHistory.Add(
                new CourtesyHistory()
                {
                    Email = CourtesyTicket.Email,
                    Date = DateTime.Now,
                    Variant = variantToGenerateTicket.Name,
                    Quantity = CourtesyTicket.quantity
                }
            );

            editEvent.Courtesy.RemainingCourtesy
                .Find(x => x.VariantId == CourtesyTicket.IdVariant)
                .Quantity -= CourtesyTicket.quantity;

            _eventRepository.Edit<Model.Event>(eventToGenerateTicket._Id, editEvent);
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
                _messageReturn.Data = (Ticket)
                    _ticketRepository.GetTickets<List<Ticket>>(ticket).Result.FirstOrDefault();
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
                var ticketUserData = (GetTicketDataUser)
                    _ticketRepository.GetTicketByIdDataUser<GetTicketDataUser>(id).Result;

                //var variantData = _variantRepository.FindById<List<Model.Variant>>(ticketUserData.Lot.FirstOrDefault().IdVariant).Result;

                _messageReturn.Data = new TicketUserDto()
                {
                    Id = ticketUserData.Id,
                    IdLot = ticketUserData.IdLot,
                    IdUser = ticketUserData.IdUser,
                    QrCode = ticketUserData.QrCode,
                    Value = ticketUserData.Value,
                    //Variante = variantData.FirstOrDefault().Name,

                    User = new UserDto()
                    {
                        _id = ticketUserData.User.FirstOrDefault()._id,
                        cpf = ticketUserData.User.FirstOrDefault().cpf,
                        email = ticketUserData.User.FirstOrDefault().email,
                        name = ticketUserData.User.FirstOrDefault().name
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

        public async Task<MessageReturn> GetTicketByIdDataEvent(string id)
        {
            try
            {
                id.ValidateIdMongo();
                var ticket = (GetTicketDataEvent)
                    _ticketRepository.GetTicketByIdDataEvent<GetTicketDataEvent>(id).Result;
                _messageReturn.Data = ticket;
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

                _messageReturn.Data = await _ticketRepository.UpdateTicketsAsync<object>(
                    id,
                    ticket
                );
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

        private async Task<string> GenerateQrCode(string idTicket)
        {
            var url = "http://api.ingressosaqui.com:3004/";
            var uri = "v1/generate-qr-code?data=" + idTicket;
            using (var httpClient = new HttpClient())
            {
                using var httpResponseMessage = await httpClient.GetAsync(url + uri);
                string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
                return jsonContent;
            }
        }

        private async Task<bool> ProcessEmail(
            string emailTicket,
            TicketEventDataDto ticketEventDto,
            string urlQrCode,
            string rowId,
            StatusTicketsRow ticketsRow,
            int index
        )
        {
            try
            {
                var email = new Email
                {
                    Body = _emailService.GenerateBody(),
                    Subject = "Ingressos",
                    Sender = "suporte@ingressosaqui.com",
                    To = emailTicket,
                    DataCadastro = DateTime.Now
                };
                //alterar pra urlQrCode
                email.Body = email.Body.Replace("{nome_usuario}", " ");
                email.Body = email.Body.Replace("{nome_evento}", ticketEventDto.@event.name);
                email.Body = email.Body.Replace(
                    "{data_evento}",
                    ticketEventDto.@event.startDate + " as " + ticketEventDto.@event.endDate
                );
                email.Body = email.Body.Replace("{local_evento}", ticketEventDto.@event.local);
                email.Body = email.Body.Replace(
                    "{endereco_evento}",
                    ticketEventDto.@event.address.addressDescription
                        + " - "
                        + ticketEventDto.@event.address.number
                        + " - "
                        + ticketEventDto.@event.address.neighborhood
                        + " - "
                        + ticketEventDto.@event.address.city
                        + " - "
                        + ticketEventDto.@event.address.state
                );
                email.Body = email.Body.Replace("{area_evento}", ticketEventDto.variant.name);
                email.Body = email.Body.Replace("{tipo_ingresso}", "Cortesia");
                email.Body = email.Body.Replace("{qr_code}", urlQrCode);

                _ = await _emailService.SaveAsync(email);
                var isEmailSend = _emailService.Send(email.id, ticketsRow, index, rowId).Result;

                if (isEmailSend.Data == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ticketsRow.TicketStatus[index].Status = TicketStatusEnum.Erro;
                ticketsRow.TicketStatus[index].Message = ex.Message;

                _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
                return false;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Liberar recursos gerenciados
                    _httpClient.Dispose();
                }

                // Liberar recursos não gerenciados, se houver

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
