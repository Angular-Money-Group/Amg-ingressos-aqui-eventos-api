using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Enum;

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
                    throw new SaveException("Id Lote é Obrigatório.");
                if (ticket?.Value == 0)
                    throw new SaveException("Valor do Ingresso é Obrigatório.");

                ticket?.IdLot?.ValidateIdMongo();

                _messageReturn.Data = await _ticketRepository.SaveAsync<object>(ticket!);
            }
            catch (SaveException ex)
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

        public async Task<MessageReturn> GetCourtesyStatus(string id)
        {
            try
            {
                _messageReturn.Data =
                    await _ticketRowRepository.GetCourtesyStatusById<StatusTicketsRow>(id);
            }
            catch (GetException ex)
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
            catch (SaveException ex)
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
            catch (DeleteException ex)
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
            catch (DeleteException ex)
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
                    TotalTickets = CourtesyTicket.Quantity,
                    TicketStatus = new List<TicketStatusResult>(),
                };

                var rowId = _ticketRowRepository.SaveRowAsync<StatusTicketsRow>(ticketsRow).Result;

                _ = ProcessEmailSending(CourtesyTicket, rowId, ticketsRow);

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
            GenerateCourtesyTicketDto courtesyTicket,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            try
            {
                await UpdateTicketStatusAsync(courtesyTicket.Quantity, ticketsRow, rowId);

                var lotToGenerateTicket = await _lotRepository.GetLotByIdVariant<Model.Lot>(
                    courtesyTicket.IdVariant
                );
                var variantToGenerateTicket = await _variantRepository.FindById<Model.Variant>(
                    courtesyTicket.IdVariant
                );
                var eventToGenerateTicket = await _eventRepository.GetByIdVariant<Model.Event>(
                    variantToGenerateTicket[0].IdEvent
                );

                var isEmailSend = await SendEmailAndUpdateStatus(
                    courtesyTicket,
                    lotToGenerateTicket,
                    variantToGenerateTicket[0],
                    eventToGenerateTicket,
                    rowId,
                    ticketsRow
                );

                if (!isEmailSend)
                {
                    HandleEmailSendingFailure(rowId, ticketsRow);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while processing email sending",
                    ex
                );
            }
        }

        private async Task UpdateTicketStatusAsync(
            int quantity,
            StatusTicketsRow ticketsRow,
            string rowId
        )
        {
            for (var i = 0; i < quantity; i++)
            {
                var ticketStatusResult = new TicketStatusResult()
                {
                    Identificate = i,
                    Status = TicketStatusProcessEnum.Processando
                };

                ticketsRow.TicketStatus.Add(ticketStatusResult);
            }

            await _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
        }

        public async Task<MessageReturn> ReSendCourtesyTickets(string rowId, string variantId)
        {
            var ticketsRow = _ticketRowRepository
                .GetCourtesyStatusById<StatusTicketsRow>(rowId)
                .Result;

            ticketsRow.TicketStatus
                .FindAll(t => t.Status == TicketStatusProcessEnum.Erro)
                .ForEach(
                    async (t) =>
                    {
                        var lotToGenerateTicket = await _lotRepository.GetLotByIdVariant<Model.Lot>(
                            variantId
                        );
                        var variantToGenerateTicket =
                            await _variantRepository.FindById<Model.Variant>(variantId);
                        var eventToGenerateTicket =
                            await _eventRepository.GetByIdVariant<Model.Event>(
                                variantToGenerateTicket[0].IdEvent
                            );

                        if (t.TicketId != null)
                        {
                            var ticket = await _ticketRepository.GetById<Ticket>(t.TicketId);

                            var ticketEventDataDto = CreateTicketEventDataDto(
                                t.TicketId,
                                lotToGenerateTicket,
                                variantToGenerateTicket[0],
                                eventToGenerateTicket,
                                ticket.QrCode
                            );

                            var isEmailSend = await ProcessEmail(
                                ticketsRow.Email,
                                ticketEventDataDto,
                                ticket.QrCode,
                                rowId,
                                ticketsRow,
                                t.Identificate
                            );
                        }
                        else
                        {
                            var idUser = ObjectId.GenerateNewId().ToString();

                            Ticket ticketToSend = CreateTicketToSend(idUser, lotToGenerateTicket);
                            var ticketId = await SaveTicketAsync(ticketToSend);

                            var qrCodeImage = UpdateTicketRowStatusAndQRCode(
                                ticketId,
                                t.Identificate,
                                rowId,
                                ticketsRow
                            ).Result;

                            var ticketEventDataDto = CreateTicketEventDataDto(
                                ticketId,
                                lotToGenerateTicket,
                                variantToGenerateTicket[0],
                                eventToGenerateTicket,
                                qrCodeImage
                            );

                            var isEmailSend = await ProcessEmail(
                                ticketsRow.Email,
                                ticketEventDataDto,
                                qrCodeImage,
                                rowId,
                                ticketsRow,
                                t.Identificate
                            );
                        }
                    }
                );

            return _messageReturn;
        }

        private async Task<bool> SendEmailAndUpdateStatus(
            GenerateCourtesyTicketDto courtesyTicket,
            Model.Lot lotToGenerateTicket,
            Model.Variant variantToGenerateTicket,
            Model.Event eventToGenerateTicket,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            var idUser = ObjectId.GenerateNewId().ToString();

            if (HasSufficientCourtesyRemaining(courtesyTicket, eventToGenerateTicket))
            {
                UpdateEventCourtesyAndRemainingQuantity(
                    eventToGenerateTicket,
                    variantToGenerateTicket,
                    courtesyTicket,
                    rowId
                );

                for (var i = 0; i < courtesyTicket.Quantity; i++)
                {
                    try
                    {
                        Ticket ticketToSend = CreateTicketToSend(idUser, lotToGenerateTicket);
                        var ticketId = await SaveTicketAsync(ticketToSend);

                        var qrCodeImage = UpdateTicketRowStatusAndQRCode(
                            ticketId,
                            i,
                            rowId,
                            ticketsRow
                        ).Result;

                        var ticketEventDataDto = CreateTicketEventDataDto(
                            ticketId,
                            lotToGenerateTicket,
                            variantToGenerateTicket,
                            eventToGenerateTicket,
                            qrCodeImage
                        );

                        var isEmailSend = await ProcessEmail(
                            courtesyTicket.Email,
                            ticketEventDataDto,
                            qrCodeImage,
                            rowId,
                            ticketsRow,
                            i
                        );

                        if (!isEmailSend)
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleEmailSendingError(i, ex, rowId, ticketsRow);
                    }
                }

                return true;
            }
            else
            {
                throw new Exception("Insufficient courtesy quantity");
            }
        }

        private bool HasSufficientCourtesyRemaining(
            GenerateCourtesyTicketDto courtesyTicket,
            Model.Event eventToGenerateTicket
        )
        {
            var remainingCourtesy = eventToGenerateTicket.Courtesy.RemainingCourtesy.Find(
                x => x.VariantId == courtesyTicket.IdVariant
            );

            return remainingCourtesy.Quantity >= courtesyTicket.Quantity;
        }

        private void UpdateEventCourtesyAndRemainingQuantity(
            Model.Event eventToGenerateTicket,
            Model.Variant variantToGenerateTicket,
            GenerateCourtesyTicketDto CourtesyTicket,
            string rowId
        )
        {
            var editEvent = eventToGenerateTicket;

            editEvent.Courtesy.CourtesyHistory.Add(
                new CourtesyHistory()
                {
                    Email = CourtesyTicket.Email,
                    Date = DateTime.Now,
                    Variant = variantToGenerateTicket.Name,
                    Quantity = CourtesyTicket.Quantity,
                    IdStatusEmail = rowId
                }
            );

            var remainingCourtesy = editEvent.Courtesy.RemainingCourtesy.Find(
                x => x.VariantId == CourtesyTicket.IdVariant
            );

            remainingCourtesy.Quantity -= CourtesyTicket.Quantity;

            _eventRepository.Edit<Model.Event>(eventToGenerateTicket.Id, editEvent);
        }

        private Ticket CreateTicketToSend(string idUser, Model.Lot lotToGenerateTicket)
        {
            return new Ticket()
            {
                IdLot = lotToGenerateTicket.Id,
                IdUser = idUser,
                Status = Enum.StatusTicket.CORTESIA,
                Value = lotToGenerateTicket.ValueTotal,
                IsSold = true,
                ReqDocs = false,
                TicketCortesia = true
            };
        }

        private async Task<string> SaveTicketAsync(Ticket ticketToSend)
        {
            var ticketResult = await SaveAsync(ticketToSend);
            return ticketResult.Data.ToString() ?? string.Empty;
        }

        private async Task<string> UpdateTicketRowStatusAndQRCode(
            string ticketId,
            int index,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            ticketsRow.TicketStatus[index].TicketId = ticketId;
            await _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);

            var nameImage = await GenerateQrCode(idTicket: ticketId);

            var qrCodeUrl = (@"https://api.ingressosaqui.com/imagens/" + nameImage).Replace("\"","");

            var ticketToSend = new Ticket() { Id = ticketId, QrCode = qrCodeUrl};

            await EditTicketsAsync(ticketId, ticketToSend);

            return qrCodeUrl;
        }

        private TicketEventDataDto CreateTicketEventDataDto(
            string ticketId,
            Model.Lot lotToGenerateTicket,
            Model.Variant variantToGenerateTicket,
            Model.Event eventToGenerateTicket,
            string nameImagem
        )
        {
            var eventLocal = eventToGenerateTicket.Local;

            var @event = new Model.Event()
            {
                Id = eventToGenerateTicket.Id,
                Name = eventToGenerateTicket.Name,
                Local = eventLocal,
                Type = eventToGenerateTicket.Type,
                Image = eventToGenerateTicket.Image,
                Description = eventToGenerateTicket.Description!,
                StartDate = eventToGenerateTicket.StartDate,
                EndDate = eventToGenerateTicket.EndDate,
                Status = eventToGenerateTicket.Status,
                Address = new Model.Address()
                {
                    Cep = eventToGenerateTicket.Address!.Cep,
                    AddressDescription = eventToGenerateTicket.Address.AddressDescription,
                    Number = eventToGenerateTicket.Address.Number,
                    Neighborhood = eventToGenerateTicket.Address.Neighborhood,
                    Complement = eventToGenerateTicket.Address.Complement,
                    ReferencePoint = eventToGenerateTicket.Address.ReferencePoint,
                    City = eventToGenerateTicket.Address.City,
                    State = eventToGenerateTicket.Address.State
                },
                IdMeansReceipt = eventToGenerateTicket.IdMeansReceipt,
                IdOrganizer = eventToGenerateTicket.IdOrganizer,
                Highlighted = eventToGenerateTicket.Highlighted
            };

            return new TicketEventDataDto()
            {
                Lot = new Model.Lot()
                {
                    Id = lotToGenerateTicket.Id!,
                    Identificate = lotToGenerateTicket.Identificate,
                    TotalTickets = lotToGenerateTicket.TotalTickets,
                    ValueTotal = lotToGenerateTicket.ValueTotal,
                    StartDateSales = lotToGenerateTicket.StartDateSales,
                    EndDateSales = lotToGenerateTicket.EndDateSales,
                    Status = lotToGenerateTicket.Status,
                    ReqDocs = lotToGenerateTicket.ReqDocs,
                    IdVariant = lotToGenerateTicket.IdVariant
                },
                Variant = new Model.Variant()
                {
                    Id = variantToGenerateTicket.Id,
                    Name = variantToGenerateTicket.Name,
                    Description = variantToGenerateTicket.Description,
                    HasPositions = variantToGenerateTicket.HasPositions,
                    Status = variantToGenerateTicket.Status,
                    IdEvent = variantToGenerateTicket.IdEvent,
                    ReqDocs = variantToGenerateTicket.ReqDocs,
                    Positions = variantToGenerateTicket.Positions
                },
                Event = @event,
                Id = variantToGenerateTicket.Id,
                IdLot = lotToGenerateTicket.Id!,
                IdUser = ticketId,
                Position = null,
                Value = lotToGenerateTicket.ValueTotal,
                IsSold = true,
                ReqDocs = false,
                QrCode = "https://api.ingressosaqui.com/imagens/" + nameImagem
            };
        }

        private void HandleEmailSendingFailure(string rowId, StatusTicketsRow ticketsRow)
        {
            for (var i = 0; i < ticketsRow.TicketStatus.Count; i++)
            {
                if (ticketsRow.TicketStatus[i].Status != TicketStatusProcessEnum.Processando)
                {
                    continue;
                }

                ticketsRow.TicketStatus[i].Status = TicketStatusProcessEnum.Erro;
                ticketsRow.TicketStatus[i].Message = "Email sending failed";

                _ = _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
            }
        }

        public async Task<MessageReturn> GetTicketByUser(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();
                var list = await _ticketRepository.GetTicketsByUser<
                    List<GetTicketDataEvent>
                >(idUser);
                _messageReturn.Data = list.Where(x=> x.IsSold);
            }
            catch (GetException ex)
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
        public async Task<MessageReturn> GetTicketByUserEvent(string idUser, string idEvent)
        {
            try
            {
                idUser.ValidateIdMongo();
                var list = await _ticketRepository.GetTicketsByUser<
                    List<GetTicketDataEvent>>(idUser);
                _messageReturn.Data = list.Where(x=> x.IsSold && x.Event.Id==idEvent);
            }
            catch (GetException ex)
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
            catch (GetException ex)
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
            catch (GetException ex)
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
            catch (GetException ex)
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
                _messageReturn = new MessageReturn();
                id.ValidateIdMongo();
                var ticketUserData = (GetTicketDataUser)
                    _ticketRepository.GetTicketByIdDataUser<GetTicketDataUser>(id).Result;

                //var variantData = _variantRepository.FindById<List<Model.Variant>>(ticketUserData.Lot.FirstOrDefault().IdVariant).Result;

                _messageReturn.Data = new TicketUserDto()
                {
                    Id = ticketUserData.Id,
                    IdLot = ticketUserData.IdLot,
                    IdUser = ticketUserData.IdUser,
                    IsSold = ticketUserData.IsSold,
                    QrCode = ticketUserData.QrCode,
                    Value = ticketUserData.Value,
                    User = new UserDto()
                    {
                        Id = ticketUserData.User.FirstOrDefault().Id,
                        Cpf = ticketUserData.User.FirstOrDefault().DocumentId,
                        Email = ticketUserData.User.FirstOrDefault().Contact.Email,
                        Name = ticketUserData.User.FirstOrDefault().Name
                    },
                };
            }
            catch (GetException ex)
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
            catch (GetException ex)
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

        public async Task<MessageReturn> EditTicketsAsync(string id, Ticket ticket)
        {
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _ticketRepository.UpdateTicketsAsync<object>(
                    id,
                    ticket
                );
            }
            catch (EditException ex)
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

                email.Body = email.Body.Replace("{nome_usuario}", " ");
                email.Body = email.Body.Replace("{nome_evento}", ticketEventDto.Event.Name);
                email.Body = email.Body.Replace(
                    "{data_evento}",
                    $"{ticketEventDto.Event.StartDate} as {ticketEventDto.Event.EndDate}"
                );
                email.Body = email.Body.Replace("{local_evento}", ticketEventDto.Event.Local);
                email.Body = email.Body.Replace(
                    "{endereco_evento}",
                    $"{ticketEventDto.Event.Address.AddressDescription} - {ticketEventDto.Event.Address.Number} - {ticketEventDto.Event.Address.Neighborhood} - {ticketEventDto.Event.Address.City} - {ticketEventDto.Event.Address.State}"
                );
                email.Body = email.Body.Replace("{area_evento}", ticketEventDto.Variant.Name);
                email.Body = email.Body.Replace("{tipo_ingresso}", "Cortesia");
                email.Body = email.Body.Replace("{qr_code}", urlQrCode);

                var savedEmailResult = _emailService.SaveAsync(email).Result;

                var isEmailSend = _emailService.Send(email.id, ticketsRow, index, rowId).Result;

                 if ((bool)isEmailSend.Data)
                {
                    ticketsRow.TicketStatus[index].Status = TicketStatusProcessEnum.Enviado;
                    _ticketRowRepository.UpdateTicketsRowAsync<Model.StatusTicketsRow>(
                        rowId,
                        ticketsRow
                    );

                    return true;
                }
                else
                {
                    _messageReturn.Message = "Erro ao enviar email";
                    HandleEmailSendingFailure(index, rowId, ticketsRow);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                HandleEmailSendingError(index, ex, rowId, ticketsRow);
                return false;
            }
        }

        private async void HandleEmailSendingFailure(
            int index,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            ticketsRow.TicketStatus[index].Status = TicketStatusProcessEnum.Erro;
            ticketsRow.TicketStatus[index].Message = "Failed to send email";

            await _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
        }

        private async void HandleEmailSendingError(
            int index,
            Exception ex,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            ticketsRow.TicketStatus[index].Status = TicketStatusProcessEnum.Erro;
            ticketsRow.TicketStatus[index].Message = ex.Message;

            await _ticketRowRepository.UpdateTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
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
