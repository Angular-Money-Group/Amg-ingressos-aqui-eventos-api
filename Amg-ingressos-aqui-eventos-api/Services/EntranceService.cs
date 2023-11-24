using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class EntranceService : IEntranceService, IDisposable
    {
        //private IEventQRReads userColabData;
        //private EntranceDto entranceData;
        //private MessageReturn _messageReturn;

        private IEntranceRepository _entranceRepository;
        private ITicketRepository _ticketRepository;
        //private HttpClient _httpClient;

        public EntranceService(IEntranceRepository entranceRepository, ITicketRepository ticketRepository)
        {
            _entranceRepository = entranceRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<MessageReturn> EntranceTicket(EntranceDto entranceDTO)
        {
            MessageReturn _messageReturn = new MessageReturn();
            User colab = new User();
            GetTicketDataEvent evento = new GetTicketDataEvent();
            GetTicketDataUser userTicket = new GetTicketDataUser();

            EventQrReads eventQrReads = new EventQrReads();

            try
            {
                //Validações dos dados do request
                #region Validações dos dados do request
                //Consulta os dados do colaborador
                colab = await _entranceRepository.GetUserColabData<User>(entranceDTO.IdColab);
                if (colab == null)
                {
                    _messageReturn.Data = "404";
                    _messageReturn.Message = "Colaborador da leitura do ingresso inválido";
                    return _messageReturn;
                }

                //Consulta os dados do evento, do qrcode (ticket) que acabou de ser lido
                evento = (GetTicketDataEvent)_ticketRepository.GetTicketByIdDataEvent<GetTicketDataEvent>(entranceDTO.IdTicket).Result;

                //Se for null, é um qrcode (ticket) inexistente
                if (evento == null)
                {
                    _messageReturn.Data = "404";
                    _messageReturn.Message = "Evento não encontrado";
                    return _messageReturn;
                }
                //Valida se o ingresso é do evento 
                else if (evento.Event.Id != entranceDTO.IdEvent)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso não pertencente ao evento";
                    return _messageReturn;
                }

                //Consulta os dados do usuários do qrcode (ticket) que acabou de ser lido
                userTicket = (GetTicketDataUser)_ticketRepository.GetTicketByIdDataUser<GetTicketDataUser>(entranceDTO.IdTicket).Result;

                //Se não encontrar dados de usúario é um qrcode(ticket) inválido
                if (userTicket == null)
                {
                    _messageReturn.Data = "404";
                    _messageReturn.Message = "Ingresso não vendido";
                    return _messageReturn;
                }
                #endregion

                //Checa o status do ticket e se não passar na validação finaliza a execução
                // 2- Registra o qrcode lido pelo colaborador
                #region Checa o status do ticket
                if (evento.Status == Enum.StatusTicket.USADO)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso já utilizado";

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

                    return _messageReturn;
                }
                else if (evento.Status == Enum.StatusTicket.NAO_DISPONIVEL)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso não disponível";

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

                    return _messageReturn;
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

                    return _messageReturn;
                }
                else if (evento.Status == Enum.StatusTicket.EXPIRADO)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso expirado";

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

                    return _messageReturn;
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
                EventQrReadsDto eventQrReadsDto = new  EventQrReadsDto(){
                        Id = eventQrReads.Id,
                        IdColab= eventQrReads.IdColab,
                        IdEvent = eventQrReads.IdEvent,
                        InitialDate= eventQrReads.InitialDate,
                        LastRead= eventQrReads.LastRead,
                        TotalFail= eventQrReads.TotalFail,
                        TotalReads= eventQrReads.TotalReads,
                        TotalSuccess= eventQrReads.TotalSuccess,
                        DocumentId = colab.DocumentId,
                        NameUser = colab.Name,
                        NameVariant= evento.Variant.Name
                    };

                //3- Da baixa (queima) no qrcode (ticket), colocando ele como utilizado
                await _ticketRepository.BurnTicketsAsync<Ticket>(entranceDTO.IdTicket, (int)Enum.StatusTicket.USADO);
                #endregion

                _messageReturn.Message = "";
                _messageReturn.Data = eventQrReadsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return _messageReturn;
        }

        internal void SaveReadyHistories(ReadHistory ticket)
        {
            try
            {
               _entranceRepository.saveReadyHistories<object>(ticket).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private EventQrReads SaveEventQrReads<T>(Boolean successQrRead, EntranceDto entranceDTO)
        {
            EventQrReads eventQrReads = new EventQrReads();
            try
            {
                //2- Consulta se o colaborador já leu algum qrCode (ticket) no evento e na data de hoje
                eventQrReads = _entranceRepository.getEventQrReads<EventQrReads>(entranceDTO.IdEvent, entranceDTO.IdColab, DateTime.Now).GetAwaiter().GetResult();

                //Se o objeto estiver null, é o primeiro QrCode lido do evento no dia
                if (eventQrReads == null)
                {
                    //Registra o qrCode lido do evento na data de hoje
                    eventQrReads = _entranceRepository.saveEventQrReads<EventQrReads>(new EventQrReads()
                    {
                        IdColab = entranceDTO.IdColab,
                        IdEvent = entranceDTO.IdEvent,
                        InitialDate = DateTime.Now,
                        Status = (int)Enum.EventQrReadsEnum.INICIADO,
                        TotalFail = successQrRead ? 0 : 1,
                        TotalReads = 1,
                        TotalSuccess = successQrRead ? 1 : 0
                        //readTicket = new List<string>() { entranceDTO.IdTicket }
                    }).GetAwaiter().GetResult();
                }
                else
                {
                    eventQrReads.LastRead = DateTime.Now;
                    eventQrReads.TotalReads++;
                    eventQrReads.TotalSuccess = successQrRead ? (eventQrReads.TotalSuccess + 1) : eventQrReads.TotalSuccess;
                    eventQrReads.TotalFail = successQrRead ? eventQrReads.TotalFail : (eventQrReads.TotalFail + 1);
                    //eventQrReads.loginHistory.Add(entranceDTO.IdTicket);
                    //eventQrReads.readHistory.Add(entranceDTO.IdTicket);

                    //Atualiza os qrCode lido do evento na data de hoje
                    eventQrReads = _entranceRepository.UpdateEventQrReads<EventQrReads>(eventQrReads).GetAwaiter().GetResult();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return eventQrReads;
        }


        private bool _disposed = false;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
