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
        private readonly IEmailService _emailService;
        private readonly IEventRepository _eventRepository;
        private MessageReturn _messageReturn;
        private readonly ILogger<TicketService> _logger;

        public TicketService(
            ITicketRepository ticketRepository,
            ITicketRowRepository ticketRowRepository,
            IVariantRepository variantRepository,
            ILotRepository lotRepository,
            IEmailService emailService,
            IEventRepository eventRepository,
            ILogger<TicketService> logger
        )
        {
            _ticketRepository = ticketRepository;
            _ticketRowRepository = ticketRowRepository;
            _variantRepository = variantRepository;
            _lotRepository = lotRepository;
            _emailService = emailService;
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
                _messageReturn.Data = await _ticketRepository.SaveAsync<object>(ticket!);
            }
            catch (SaveException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Ticket"));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetCourtesyStatusById(string id)
        {
            try
            {
                _messageReturn.Data = await _ticketRowRepository.GetCourtesyStatusById<StatusTicketsRow>(id);
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCourtesyStatusById), "Cortesia"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCourtesyStatusById), "Cortesia"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCourtesyStatusById), "Cortesia"));
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
            catch (SaveException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SaveManyAsync), "Cortesia"), listTicket);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SaveManyAsync), "Cortesia"), listTicket);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SaveManyAsync), "Cortesia"), listTicket);
                throw;
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "tickets"), tickets);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "tickets"), tickets);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteTicketsByLot(string lotId)
        {
            try
            {
                lotId.ValidateIdMongo();
                _messageReturn.Data = await _ticketRepository.DeleteByLot<object>(lotId);
            }
            catch (DeleteException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteTicketsByLot), "ticket"), lotId);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteTicketsByLot), "ticket"), lotId);
                throw;
            }

            return _messageReturn;
        }

        public MessageReturn SendCourtesyTickets(GenerateCourtesyTicketDto courtesyTicket)
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

                var rowId = _ticketRowRepository.SaveRowAsync<StatusTicketsRow>(ticketsRow).Result;

                _ = ProcessEmailSending(courtesyTicket, rowId, ticketsRow);

                _messageReturn.Data = "Enviando...";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SendCourtesyTickets), "cortesia"), courtesyTicket);
                _messageReturn.Message = ex.Message;
                throw;
            }

            return _messageReturn;
        }

        public async Task ProcessEmailSending(GenerateCourtesyTicketDto courtesyTicket, string rowId, StatusTicketsRow ticketsRow)
        {
            try
            {
                await UpdateTicketStatusAsync(courtesyTicket.Quantity, ticketsRow, rowId);

                var lotToGenerateTicket = await _lotRepository.GetLotByIdVariant<Lot>(
                    courtesyTicket.IdVariant
                );
                var variantToGenerateTicket = await _variantRepository.GetById<Variant>(
                    courtesyTicket.IdVariant
                );
                var eventToGenerateTicket = await _eventRepository.GetById<Event>(
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
                    HandleEmailSendingFailure(0, rowId, ticketsRow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(ProcessEmailSending), "email cortesia"), courtesyTicket);
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
                    Status = EnumTicketStatusProcess.Processando
                };

                ticketsRow.TicketStatus.Add(ticketStatusResult);
            }

            await _ticketRowRepository.EditTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
        }

        public MessageReturn ReSendCourtesyTickets(string rowId, string variantId)
        {
            var ticketsRow = _ticketRowRepository
                .GetCourtesyStatusById<StatusTicketsRow>(rowId)
                .Result;

            ticketsRow.TicketStatus
                .FindAll(t => t.Status == EnumTicketStatusProcess.Erro)
                .ForEach(
                    async (t) =>
                    {
                        var lotToGenerateTicket = await _lotRepository.GetLotByIdVariant<Model.Lot>(
                            variantId
                        );
                        var variantToGenerateTicket =
                            await _variantRepository.GetById<Variant>(variantId);
                        var eventToGenerateTicket =
                            await _eventRepository.GetById<Event>(variantToGenerateTicket[0].IdEvent);

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
                                variantToGenerateTicket[0],
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
            GenerateCourtesyTicketDto courtesyTicket,
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
                        _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SendEmailAndUpdateStatus), "enviar email e atualizar status"));
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
            GenerateCourtesyTicketDto courtesyTicket,
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
            GenerateCourtesyTicketDto CourtesyTicket,
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
                Status = EnumStatusTicket.CORTESIA,
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
            await _ticketRowRepository.EditTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);

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
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUser), "ticket"), idUser);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUser), "ticket"), idUser);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUser), "ticket"), idUser);
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
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUserAndEvent), "ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUserAndEvent), "ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByUserAndEvent), "ticket"));
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
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetTicketsByLot), "ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetTicketsByLot), "ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetTicketsByLot), "ticket"));
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
                var result = await _ticketRepository.GetTickets<List<Ticket>>(ticket);
                _messageReturn.Data = result.Where(i => i.IdUser == null).ToList();
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetRemainingByLot), "ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetRemainingByLot), "ticket"));
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetRemainingByLot), "ticket"));
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
                _messageReturn.Data = ticketResult?.FirstOrDefault() ?? new Ticket();
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetById), "ticket"), id);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetById), "ticket"), id);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetById), "ticket"), id);
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
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataUser), "ticket"), id);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataUser), "ticket"), id);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataUser), "ticket"), id);
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
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataEvent), "ticket"), id);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataEvent), "ticket"), id);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdWithDataEvent), "ticket"), id);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> EditAsync(string id, Ticket ticket)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _ticketRepository.EditAsync<object>(id, ticket);
            }
            catch (EditException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), "ticket"), ticket);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), "ticket"), ticket);
                _messageReturn.Message = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), "ticket"), ticket);
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
                    $"{ticketEventDto?.Event?.Address?.AddressDescription} - {ticketEventDto?.Event?.Address?.Number} - {ticketEventDto?.Event?.Address?.Neighborhood} - {ticketEventDto?.Event?.Address?.City} - {ticketEventDto?.Event?.Address?.State}"
                );
                email.Body = email.Body.Replace("{area_evento}", ticketEventDto?.Variant.Name);
                email.Body = email.Body.Replace("{tipo_ingresso}", "Cortesia");
                email.Body = email.Body.Replace("{qr_code}", urlQrCode);

                await _emailService.SaveAsync(email);

                var isEmailSend = _emailService.Send(email?.Id ?? string.Empty, ticketsRow, index, rowId).Result;

                if ((bool)isEmailSend.Data)
                {
                    ticketsRow.TicketStatus[index].Status = EnumTicketStatusProcess.Enviado;
                    await _ticketRowRepository.EditTicketsRowAsync<StatusTicketsRow>(
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
            ticketsRow.TicketStatus[index].Status = EnumTicketStatusProcess.Erro;
            ticketsRow.TicketStatus[index].Message = "Failed to send email";

            _ticketRowRepository.EditTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
        }

        private void HandleEmailSendingError(
            int index,
            Exception ex,
            string rowId,
            StatusTicketsRow ticketsRow
        )
        {
            ticketsRow.TicketStatus[index].Status = EnumTicketStatusProcess.Erro;
            ticketsRow.TicketStatus[index].Message = ex.Message;

            _ticketRowRepository.EditTicketsRowAsync<StatusTicketsRow>(rowId, ticketsRow);
        }
    }
}