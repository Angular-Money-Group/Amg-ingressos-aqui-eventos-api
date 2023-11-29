using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class EntranceService : IEntranceService
    {
        private IEntranceRepository _entranceRepository;
        private ITicketRepository _ticketRepository;
        private MessageReturn _messageReturn;
        private ILogger<EntranceService> _logger;

        public EntranceService(
            IEntranceRepository entranceRepository,
            ITicketRepository ticketRepository,
            ILogger<EntranceService> logger)
        {
            _entranceRepository = entranceRepository;
            _ticketRepository = ticketRepository;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> EntranceTicket(EntranceDto entranceDTO)
        {

            EventQrReads eventQrReads = new EventQrReads();

            try
            {
                //Validações dos dados do request
                #region Validações dos dados do request
                //Consulta os dados do colaborador
                var colab = await _entranceRepository.GetUserColabData<User>(entranceDTO.IdColab);
                if (colab == null)
                    throw new GetException("Colaborador da leitura do ingresso inválido");

                //Consulta os dados do evento, do qrcode (ticket) que acabou de ser lido
                var evento = (GetTicketDataEvent)_ticketRepository.GetTicketByIdDataEvent<GetTicketDataEvent>(entranceDTO.IdTicket).Result;

                //Se for null, é um qrcode (ticket) inexistente
                if (evento == null)
                    throw new GetException("Evento não encontrado");

                //Valida se o ingresso é do evento 
                else if (evento.Event.Id != entranceDTO.IdEvent)
                    throw new RuleException("Ingresso não pertencente ao evento");

                //Consulta os dados do usuários do qrcode (ticket) que acabou de ser lido
                var userTicket = (GetTicketDataUser)_ticketRepository.GetTicketByIdDataUser<GetTicketDataUser>(entranceDTO.IdTicket).Result;

                //Se não encontrar dados de usúario é um qrcode(ticket) inválido
                if (userTicket == null)
                    throw new GetException("Ingresso não vendido");
                #endregion

                //Checa o status do ticket e se não passar na validação finaliza a execução
                // 2- Registra o qrcode lido pelo colaborador
                #region Checa o status do ticket
                if (evento.Status == Enum.StatusTicket.USADO)
                {

                    //Insere tabela de historico de leituras de qrcode (ticket)
                    SaveReadyHistories(new ReadHistory()
                    {
                        IdEvent = entranceDTO.IdEvent,
                        IdTicket = entranceDTO.IdTicket,
                        IdColab = entranceDTO.IdColab,
                        Reason = _messageReturn.Message,
                        Status = (int)Enum.StatusTicket.USADO,
                        Date = DateTime.Now
                    });

                    //2- Salva a leitura com sucesso do qrCode
                    eventQrReads = SaveEventQrReads<EventQrReads>(false, entranceDTO);

                    throw new RuleException("Ingresso já utilizado");
                }
                else if (evento.Status == Enum.StatusTicket.NAO_DISPONIVEL)
                {
                    //Insere tabela de historico de leituras de qrcode (ticket)
                    SaveReadyHistories(new ReadHistory()
                    {
                        IdEvent = entranceDTO.IdEvent,
                        IdTicket = entranceDTO.IdTicket,
                        IdColab = entranceDTO.IdColab,
                        Reason = _messageReturn.Message,
                        Status = (int)Enum.StatusTicket.NAO_DISPONIVEL,
                        Date = DateTime.Now
                    });

                    //2- Salva a leitura com sucesso do qrCode
                    eventQrReads = SaveEventQrReads<EventQrReads>(false, entranceDTO);

                    throw new RuleException("Ingresso não disponível");
                }
                else if (evento.Status == Enum.StatusTicket.NAO_VENDIDO)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso não vendido";

                    //Insere tabela de historico de leituras de qrcode (ticket)
                    SaveReadyHistories(new ReadHistory()
                    {
                        IdEvent = entranceDTO.IdEvent,
                        IdTicket = entranceDTO.IdTicket,
                        IdColab = entranceDTO.IdColab,
                        Reason = _messageReturn.Message,
                        Status = (int)Enum.StatusTicket.NAO_VENDIDO,
                        Date = DateTime.Now
                    });

                    //2- Salva a leitura com sucesso do qrCode
                    eventQrReads = SaveEventQrReads<EventQrReads>(false, entranceDTO);

                    throw new RuleException("Ingresso não vendido");
                }
                else if (evento.Status == Enum.StatusTicket.EXPIRADO)
                {
                    //Insere tabela de historico de leituras de qrcode (ticket)
                    SaveReadyHistories(new ReadHistory()
                    {
                        IdEvent = entranceDTO.IdEvent,
                        IdTicket = entranceDTO.IdTicket,
                        IdColab = entranceDTO.IdColab,
                        Reason = _messageReturn.Message,
                        Status = (int)Enum.StatusTicket.EXPIRADO,
                        Date = DateTime.Now
                    });

                    //2- Salva a leitura com sucesso do qrCode
                    eventQrReads = SaveEventQrReads<EventQrReads>(false, entranceDTO);

                    throw new RuleException("Ingresso expirado");
                }
                #endregion

                //Confirma a leitura do qrcode (ticket) no evento pelo colaborador
                // 1- Salva o log de leitura com sucesso do qrcode (ticket)
                // 2- Registra o qrcode lido pelo colaborador
                // 3- Da baixa no qrcode (ticket), colocando ele como utilizado
                #region Confirma a leitura do qrcode (ticket) no evento pelo colaborador
                //1- Insere tabela de historico de leituras de qrcode (ticket)
                SaveReadyHistories(new ReadHistory()
                {
                    IdTicket = entranceDTO.IdTicket,
                    IdColab = entranceDTO.IdColab,
                    IdEvent = entranceDTO.IdEvent,
                    Reason = "Ingresso lido com sucesso",
                    Status = (int)Enum.StatusTicket.DISPONIVEL,
                    Date = DateTime.Now
                });

                //2- Salva a leitura com sucesso do qrCode
                eventQrReads = SaveEventQrReads<EventQrReads>(true, entranceDTO);
                EventQrReadsDto eventQrReadsDto = new EventQrReadsDto()
                {
                    Id = eventQrReads.Id,
                    IdColab = eventQrReads.IdColab,
                    IdEvent = eventQrReads.IdEvent,
                    InitialDate = eventQrReads.InitialDate,
                    LastRead = eventQrReads.LastRead,
                    TotalFail = eventQrReads.TotalFail,
                    TotalReads = eventQrReads.TotalReads,
                    TotalSuccess = eventQrReads.TotalSuccess,
                    DocumentId = colab.DocumentId,
                    NameUser = colab.Name,
                    NameVariant = evento.Variant.Name
                };

                //3- Da baixa (queima) no qrcode (ticket), colocando ele como utilizado
                await _ticketRepository.BurnTicketsAsync<Ticket>(entranceDTO.IdTicket, (int)Enum.StatusTicket.USADO);
                #endregion

                _messageReturn.Data = eventQrReadsDto;
            }
            catch
            {
                throw;
            }

            return _messageReturn;
        }

        internal void SaveReadyHistories(ReadHistory readHistory)
        {
            try
            {
                if (readHistory == null){
                    throw new SaveException("ticket readHistory é obrigatório");
                }

                _entranceRepository.SaveReadyHistories<object>(readHistory).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveReadyHistories), "ReadyHisotory"), readHistory);
                throw;
            }
        }

        private EventQrReads SaveEventQrReads<T>(Boolean successQrRead, EntranceDto entranceDTO)
        {
            try
            {
                if (entranceDTO == null)
                    throw new SaveException("Entrance Dto é obrigátorio");

                //2- Consulta se o colaborador já leu algum qrCode (ticket) no evento e na data de hoje
                var eventQrReads = _entranceRepository.GetEventQrReads<EventQrReads>(entranceDTO.IdEvent, entranceDTO.IdColab, DateTime.Now).GetAwaiter().GetResult();

                //Se o objeto estiver null, é o primeiro QrCode lido do evento no dia
                if (eventQrReads == null)
                {
                    //Registra o qrCode lido do evento na data de hoje
                    eventQrReads = _entranceRepository.SaveEventQrReads<EventQrReads>(new EventQrReads()
                    {
                        IdColab = entranceDTO.IdColab,
                        IdEvent = entranceDTO.IdEvent,
                        InitialDate = DateTime.Now,
                        Status = (int)Enum.EventQrReadsEnum.INICIADO,
                        TotalFail = successQrRead ? 0 : 1,
                        TotalReads = 1,
                        TotalSuccess = successQrRead ? 1 : 0
                    }).GetAwaiter().GetResult();
                }
                else
                {
                    eventQrReads.LastRead = DateTime.Now;
                    eventQrReads.TotalReads++;
                    eventQrReads.TotalSuccess = successQrRead ? (eventQrReads.TotalSuccess + 1) : eventQrReads.TotalSuccess;
                    eventQrReads.TotalFail = successQrRead ? eventQrReads.TotalFail : (eventQrReads.TotalFail + 1);

                    //Atualiza os qrCode lido do evento na data de hoje
                    eventQrReads = _entranceRepository.UpdateEventQrReads<EventQrReads>(eventQrReads).GetAwaiter().GetResult();
                }

                return eventQrReads;
            }
            catch(SaveException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveEventQrReads), "Event QrRead"), entranceDTO);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveEventQrReads), "Event QrRead"), entranceDTO);
                throw;
            }
        }
    }
}