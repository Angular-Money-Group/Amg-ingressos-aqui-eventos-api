using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Dto;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Enum;
using Amg_ingressos_aqui_eventos_api.Consts;
using System.Data;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketRowRepository _ticketRowRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly ILotRepository _lotRepository;
        private readonly INotificationService _notificationService;
        private readonly IEventRepository _eventRepository;
        private MessageReturn _messageReturn;
        private readonly ILogger<TicketService> _logger;

        public TicketService(
            ITicketRepository ticketRepository,
            ITicketRowRepository ticketRowRepository,
            IVariantRepository variantRepository,
            ILotRepository lotRepository,
            INotificationService notificationService,
            IEventRepository eventRepository,
            ILogger<TicketService> logger
        )
        {
            _ticketRepository = ticketRepository;
            _ticketRowRepository = ticketRowRepository;
            _variantRepository = variantRepository;
            _lotRepository = lotRepository;
            _notificationService = notificationService;
            _eventRepository = eventRepository;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(Ticket ticket)
        {
            try
            {
                if (ticket.IdLot == null)
                    throw new SaveException("Id Lote é Obrigatório.");
                if (ticket.Value == 0)
                    throw new SaveException("Valor do Ingresso é Obrigatório.");

                ticket.IdLot.ValidateIdMongo();
                _messageReturn.Data = await _ticketRepository.Save(ticket!);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Ticket"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetCourtesyStatusById(string id)
        {
            try
            {
                _messageReturn.Data = await _ticketRowRepository.GetCourtesyStatusById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCourtesyStatusById), "Cortesia"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> SaveManyAsync(List<Ticket> listTicket)
        {
            try
            {
                _messageReturn.Data = await _ticketRepository.SaveMany(listTicket);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SaveManyAsync), "Cortesia"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(List<string> tickets)
        {
            try
            {
                _messageReturn.Data = await _ticketRepository.DeleteMany(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "tickets"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteTicketsByLot(string lotId)
        {
            try
            {
                lotId.ValidateIdMongo();
                _messageReturn.Data = await _ticketRepository.DeleteByLot(lotId);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteTicketsByLot), "ticket"), ex);
                throw;
            }

            return _messageReturn;
        }

        public MessageReturn SendCourtesyTickets(CourtesyTicketDto courtesyTicket)
        {
            _messageReturn = new MessageReturn();
            try
            {
                var ticketsRow = new StatusTicketsRow()
                {
                    Email = courtesyTicket.Email,
                    TotalTickets = courtesyTicket.Quantity,
                    TicketStatus = new List<TicketStatusResult>(),
                };

                var rowId = _ticketRowRepository.SaveRowAsync(ticketsRow).Result;

                _ = ProcessEmailSending(courtesyTicket, rowId, ticketsRow);

                _messageReturn.Data = "Enviando...";
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SendCourtesyTickets), "cortesia"), ex);
                _messageReturn.Message = ex.Message;
                throw;
            }

            return _messageReturn;
        }

        public async Task ProcessEmailSending(CourtesyTicketDto courtesyTicket, string rowId, StatusTicketsRow ticketsRow)
        {
            try
            {
                await UpdateTicketStatusAsync(courtesyTicket.Quantity, ticketsRow, rowId);

                var lotToGenerateTicket = await _lotRepository.GetLotByIdVariant<Lot>(
                    courtesyTicket.IdVariant
                );
                var variantToGenerateTicket = await _variantRepository.GetById(
                    courtesyTicket.IdVariant
                );
                var eventToGenerateTicket = await _eventRepository.GetById<Event>(
                    variantToGenerateTicket.IdEvent
                );

                var isEmailSend = await SendEmailAndUpdateStatus(
                    courtesyTicket,
                    lotToGenerateTicket,
                    variantToGenerateTicket,
                    eventToGenerateTicket,
                    rowId,
                    ticketsRow
                );

                if (!isEmailSend)
                {
                    HandleEmailSendingFailure(0, rowId, ticketsRow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(ProcessEmailSending), "email cortesia"), ex);
                throw;
            }
        }

        private async Task UpdateTicketStatusAsync(int quantity, StatusTicketsRow ticketsRow, string rowId)
        {
            for (var i = 0; i < quantity; i++)
            {
                var ticketStatusResult = new TicketStatusResult()
                {
                    Identificate = i,
                    Status = TicketStatusProcess.Processando
                };

                ticketsRow.TicketStatus.Add(ticketStatusResult);
            }

            await _ticketRowRepository.EditTicketsRowAsync(rowId, ticketsRow);
        }

        public MessageReturn ReSendCourtesyTickets(string rowId, string variantId)
        {
            var ticketsRow = _ticketRowRepository
                .GetCourtesyStatusById(rowId)
                .Result;

            ticketsRow.TicketStatus
                .FindAll(t => t.Status == TicketStatusProcess.Erro)
                .ForEach(
                    async (t) =>
                    {
                        var lotToGenerateTicket = await _lotRepository.GetLotByIdVariant<Model.Lot>(
                            variantId
                        );
                        var variantToGenerateTicket =
                            await _variantRepository.GetById(variantId);
                        var eventToGenerateTicket =
                            await _eventRepository.GetById(variantToGenerateTicket.IdEvent);

                        if (t.TicketId != null)
                        {
                            var ticket = await _ticketRepository.GetById(t.TicketId);

                            var ticketEventDataDto = CreateTicketEventDataDto(
                                t.TicketId,
                                lotToGenerateTicket,
                                variantToGenerateTicket,
                                eventToGenerateTicket,
                                ticket.QrCode
                            );

                            await ProcessEmail(
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
                                variantToGenerateTicket,
                                eventToGenerateTicket,
                                qrCodeImage
                            );

                            await ProcessEmail(
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
            CourtesyTicketDto courtesyTicket,
            Lot lotToGenerateTicket,
            Variant variantToGenerateTicket,
            Event eventToGenerateTicket,
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
                        _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SendEmailAndUpdateStatus), "enviar email e atualizar status"), ex);
                        HandleEmailSendingError(i, ex, rowId, ticketsRow);
                    }
                }
                return true;
            }
            else
            {
                throw new RuleException("Insufficient courtesy quantity");
            }
        }

        private bool HasSufficientCourtesyRemaining(
            CourtesyTicketDto courtesyTicket,
            Event eventToGenerateTicket
        )
        {
            var remainingCourtesy = eventToGenerateTicket.Courtesy?.RemainingCourtesy.Find(
                x => x.VariantId == courtesyTicket.IdVariant
            );

            return remainingCourtesy?.Quantity >= courtesyTicket.Quantity;
        }

        private void UpdateEventCourtesyAndRemainingQuantity(
            Event eventToGenerateTicket,
            Variant variantToGenerateTicket,
            CourtesyTicketDto CourtesyTicket,
            string rowId
        )
        {
            Event editEvent = eventToGenerateTicket ?? throw new RuleException("evento nao pode ser null");

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

            RemainingCourtesy remainingCourtesy = editEvent.Courtesy.RemainingCourtesy.Find(
                x => x.VariantId == CourtesyTicket.IdVariant
            ) ?? new RemainingCourtesy();

            remainingCourtesy.Quantity -= CourtesyTicket.Quantity;

            _eventRepository.Edit(eventToGenerateTicket.Id, editEvent);
        }

        private Ticket CreateTicketToSend(string idUser, Lot lotToGenerateTicket)
        {
            if (lotToGenerateTicket == null)
                throw new RuleException("Lote Nao pode ser null");

            return new Ticket()
            {
                IdLot = lotToGenerateTicket.Id,
                IdUser = idUser,
                Status = StatusTicket.CORTESIA,
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
            await _ticketRowRepository.EditTicketsRowAsync(rowId, ticketsRow);

            var nameImage = await GenerateQrCode(idTicket: ticketId);

            var qrCodeUrl = (@"https://api.ingressosaqui.com/imagens/" + nameImage).Replace("\"", "");

            var ticketToSend = new Ticket() { Id = ticketId, QrCode = qrCodeUrl };

            await EditAsync(ticketId, ticketToSend);

            return qrCodeUrl;
        }

        private TicketEventDataDto CreateTicketEventDataDto(
            string ticketId,
            Lot lotToGenerateTicket,
            Variant variantToGenerateTicket,
            Event eventToGenerateTicket,
            string nameImagem
        )
        {
            var eventLocal = eventToGenerateTicket.Local;

            var @event = new Event()
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
                Address = new Address()
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
                Lot = new Lot()
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
                Variant = new Variant()
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
                Position = string.Empty,
                Value = lotToGenerateTicket.ValueTotal,
                IsSold = true,
                ReqDocs = false,
                QrCode = "https://api.ingressosaqui.com/imagens/" + nameImagem
            };
        }

        public async Task<MessageReturn> GetByUser(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();
                var data = await _ticketRepository.GetByUser<TicketComplet>(idUser);
                var list = new TicketCompletDto().ModelListToDtoList(data);
                _messageReturn.Data = list.Where(x => x.IsSold);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUser), "ticket"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetByUserAndEvent(string idUser, string idEvent)
        {
            try
            {
                idUser.ValidateIdMongo();
                var data = await _ticketRepository.GetByUser<TicketComplet>(idUser);
                var list = new TicketCompletDto().ModelListToDtoList(data);
                _messageReturn.Data = list.Where(x => x.IsSold && x.Event.Id == idEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUserAndEvent), ex));
                throw;
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
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetTicketsByLot), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetRemainingByLot(string idLot)
        {
            try
            {
                idLot.ValidateIdMongo();
                var ticket = new Ticket() { IdLot = idLot };
                var result = await _ticketRepository.GetTickets<Ticket>(ticket);
                _messageReturn.Data = result.Where(i => i.IdUser == null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetRemainingByLot), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetById(string id)
        {
            try
            {
                id.ValidateIdMongo();
                var ticket = new Ticket() { Id = id };
                var ticketResult = await _ticketRepository.GetTickets<List<Ticket>>(ticket);
                _messageReturn.Data = ticketResult?.FirstOrDefault() ?? new List<Ticket>();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetById), "ticket"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetByIdWithDataUser(string id)
        {
            try
            {
                id.ValidateIdMongo();
                var data = await _ticketRepository.GetByIdWithDataUser<TicketComplet>(id);
                _messageReturn.Data = data.Any() ? new TicketUserDto().ModelToDto(data[0]) : new TicketUserDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataUser), "ticket"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetByIdWithDataEvent(string id)
        {
            try
            {
                id.ValidateIdMongo();
                var data = await _ticketRepository.GetByIdWithDataEvent<TicketComplet>(id);
                _messageReturn.Data = new TicketCompletDto().ModelToDto(data[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataEvent), "ticket"), ex);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> EditAsync(string id, Ticket ticket)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _ticketRepository.Edit(id, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), "ticket"), ex);
                throw;
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
                var notification = new EmailTicketDto()
                {
                    AddressEvent = ticketEventDto.Event.Address.AddressDescription
                    + " - "
                    + ticketEventDto.Event.Address.Number
                    + " - "
                    + ticketEventDto.Event.Address.Neighborhood
                    + " - "
                    + ticketEventDto.Event.Address.City
                    + " - "
                    + ticketEventDto.Event.Address.State,
                    EndDateEvent = ticketEventDto.Event.EndDate.ToString(),
                    EventName = ticketEventDto.Event.Name,
                    LocalEvent = ticketEventDto.Event.Local,
                    Sender = "suporte@ingressosaqui.com",
                    StartDateEvent = ticketEventDto.Event.StartDate.ToString(),
                    Subject = "Ingressos",
                    To = emailTicket,
                    TypeTicket = "Cortesia",
                    UrlQrCode = urlQrCode,
                    UserName = "",
                    VariantName = ticketEventDto.Variant.Name,

                };

                await _notificationService.SaveAsync(notification);

                ticketsRow.TicketStatus[index].Status = TicketStatusProcess.Enviado;
                await _ticketRowRepository.EditTicketsRowAsync(
                    rowId,
                    ticketsRow
                );

                return true;
            }
            catch (Exception ex)
            {
                HandleEmailSendingError(index, ex, rowId, ticketsRow);
                return false;
            }
        }

        private void HandleEmailSendingFailure(
            int index,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            ticketsRow.TicketStatus[index].Status = TicketStatusProcess.Erro;
            ticketsRow.TicketStatus[index].Message = "Failed to send email";

            _ticketRowRepository.EditTicketsRowAsync(rowId, ticketsRow);
        }

        private void HandleEmailSendingError(
            int index,
            Exception ex,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            ticketsRow.TicketStatus[index].Status = TicketStatusProcess.Erro;
            ticketsRow.TicketStatus[index].Message = ex.Message;

            _ticketRowRepository.EditTicketsRowAsync(rowId, ticketsRow);
        }
    }
}