using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Repository;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Utils;
using System.Net.Http;

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

        public async Task<MessageReturn> entranceTicket(EntranceDto entranceDTO)
        {
            MessageReturn _messageReturn = new MessageReturn();
            User colab = new User();
            GetTicketDataEvent evento = new GetTicketDataEvent();
            GetTicketDataUser userTicket = new GetTicketDataUser();
            try
            {
                //Consulta os dados do colaborador
                colab = await _entranceRepository.getUserColabData<User>(entranceDTO.IdUser);
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

                //Consulta os dados do usuários do qrcode (ticket) que acabou de ser lido
                userTicket = (GetTicketDataUser)_ticketRepository.GetTicketByIdDataUser<GetTicketDataUser>(entranceDTO.IdTicket).Result;

                //Se não encontrar dados de usúario é um qrcode(ticket) inválido
                if (userTicket == null)
                {
                    _messageReturn.Data = "404";
                    _messageReturn.Message = "Usuário não encontrado";
                    return _messageReturn;
                }

                //Valida se o ingresso é do evento que o colaborador leu o qrcode
                if (evento.Event._id != entranceDTO.IdEvent)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso não pertencente ao evento";
                    return _messageReturn;
                }

                //Checa o status do ticket e se não passar na validação finaliza a execução
                #region Checa o status do ticket
                if (evento.Status == Enum.StatusTicket.USADO)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso já utilizado";
                    return _messageReturn;
                }
                else if (evento.Status == Enum.StatusTicket.NAO_DISPONIVEL)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso não disponível";
                    return _messageReturn;
                }
                else if (evento.Status == Enum.StatusTicket.NAO_VENDIDO)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso não vendido";
                    return _messageReturn;
                }
                else if (evento.Status == Enum.StatusTicket.EXPIRADO)
                {
                    _messageReturn.Data = "400";
                    _messageReturn.Message = "Ingresso expirado";
                    return _messageReturn;
                }
                #endregion

                //Insere tabela de historico de leituras de qrcode (ticket)
                SaveReadyHistories(new ReadHistory()
                {
                    idTicket = entranceDTO.IdTicket,
                    idUser = entranceDTO.IdUser,
                    reason = "Ingresso lido com sucesso",
                    status = (int)TicketStatusEnum.Processando,
                    date = DateTime.Now
                });

                //Confirma a leitura do qrcode (ticket) no evento pelo colaborador
                // 1- Registra o qrcode lido pelo colaborador
                // 2- Da baixa no qrcode (ticket), colocando ele como utilizado
                #region Confirma a leitura do qrcode (ticket) no evento pelo colaborador

                //1- Consulta se o colaborador já leu algum qrCode (ticket) no evento e na data de hoje
                var eventQrReads = await _entranceRepository.getEventQrReads<EventQrReads>(entranceDTO.IdEvent, entranceDTO.IdUser, DateTime.Now);

                //Se o objeto estiver null, é o primeiro QrCode lido do evento no dia
                if (eventQrReads == null)
                {
                    //Registra o qrCode lido do evento na data de hoje
                    eventQrReads = await _entranceRepository.saveEventQrReads<EventQrReads>(new EventQrReads()
                    {
                        idColab = entranceDTO.IdUser,
                        idEvent = entranceDTO.IdEvent,
                        initialDate = DateTime.Now,
                        status = (int)Enum.EventQrReadsEnum.INICIADO,
                        totalFail = 0,
                        totalReads = 1,
                        totalSuccess = 1,
                        loginHistory = new List<string>() { entranceDTO.IdTicket },
                        readHistory = new List<string>() { entranceDTO.IdTicket }
                    });
                }
                else
                {
                    eventQrReads.lastRead = DateTime.Now;
                    eventQrReads.totalReads++;
                    eventQrReads.totalSuccess++;
                    eventQrReads.loginHistory.Add(entranceDTO.IdTicket);
                    eventQrReads.readHistory.Add(entranceDTO.IdTicket);

                    //Atualiza os qrCode lido do evento na data de hoje
                    eventQrReads = await _entranceRepository.UpdateEventQrReads<EventQrReads>(eventQrReads);
                }

                //2- Da baixa no qrcode (ticket), colocando ele como utilizado
                await _ticketRepository.UpdateTicketsAsync<Ticket>(entranceDTO.IdTicket, new Ticket() { 
                    Status = Enum.StatusTicket.USADO });
                #endregion

                _messageReturn.Message = "";
                _messageReturn.Data = eventQrReads;
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
                _entranceRepository.saveReadyHistories<object>(ticket);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private bool _disposed = false;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
