using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_eventos_api.Infra;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Net.Http.Headers;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class EmailService : IEmailService
    {
        private MessageReturn _messageReturn;
        private IEmailRepository _emailRepository;
        private ITicketRowRepository _ticketRowRepository;
        private HttpClient _HttpClient;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IEmailRepository emailRepository,
            ITicketRowRepository ticketRowRepository,
            ILogger<EmailService> logger
        )
        {
            _HttpClient = new HttpClient();
            _HttpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _HttpClient.Timeout = TimeSpan.FromMinutes(10);
            _logger = logger;
            _emailRepository = emailRepository;
            _ticketRowRepository = ticketRowRepository;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(Email email)
        {
            _logger.LogInformation(string.Format("Init - Save: {0}", this.GetType().Name));
            try
            {
                _logger.LogInformation(
                    string.Format("Save Repository - Save: {0}", this.GetType().Name)
                );
                _messageReturn.Data = await _emailRepository.SaveAsync(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _logger.LogInformation(string.Format("Finished - Save: {0}", this.GetType().Name));
            return _messageReturn;
        }

        public async Task<MessageReturn> Send(
            string idEmail,
            Model.StatusTicketsRow ticketsRow,
            int index,
            string rowId
        )
        {
            try
            {
                _logger.LogInformation(string.Format("Init - Send: {0}", this.GetType().Name));
                var jsonBody = new StringContent(
                    JsonSerializer.Serialize(new { emailID = idEmail }),
                    Encoding.UTF8,
                    Application.Json
                ); // using static System.Net.Mime.MediaTypeNames;
                var url = "http://api.ingressosaqui.com:3006/";
                var uri = "v1/email/";

                _logger.LogInformation(
                    string.Format("Call PostAsync - Send: {0}", this.GetType().Name)
                );
                HttpResponseMessage response = await _HttpClient.PostAsync(url + uri, jsonBody);

                if (response.IsSuccessStatusCode)
                {

                    ticketsRow.TicketStatus[index].Status = TicketStatusEnum.Enviado;
                    _ticketRowRepository.UpdateTicketsRowAsync<Model.StatusTicketsRow>(
                        rowId,
                        ticketsRow
                    );
                }
                else
                {
                    ticketsRow.TicketStatus[index].Status = TicketStatusEnum.Erro;
                    ticketsRow.TicketStatus[index].Message = response.Content
                        .ReadAsStringAsync()
                        .Result;

                    _ticketRowRepository.UpdateTicketsRowAsync<Model.StatusTicketsRow>(
                        rowId,
                        ticketsRow
                    );

                    _messageReturn.Message = "Erro ao enviar email";
                }

                _messageReturn.Data = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                ticketsRow.TicketStatus[index].Status = TicketStatusEnum.Erro;
                ticketsRow.TicketStatus[index].Message = ex.Message;

                _ticketRowRepository.UpdateTicketsRowAsync<Model.StatusTicketsRow>(
                    rowId,
                    ticketsRow
                );

                _messageReturn.Message = ex.Message;
            }

            return _messageReturn;
        }

        public string GenerateBody()
        {
            try
            {
                _logger.LogInformation(
                    string.Format("Init - GenerateBody: {0}", this.GetType().Name)
                );
                var path = (Environment.CurrentDirectory + "/Template/index.html");
                var html = System.IO.File.ReadAllText(path);
                var body = html;
                _logger.LogInformation(
                    string.Format("Finished - GenerateBody: {0}", this.GetType().Name)
                );
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    string.Format(
                        "error - GenerateBody: {0},message: {1}",
                        this.GetType().Name,
                        ex.Message
                    ),
                    ex
                );
                throw ex;
            }
        }
    }
}
