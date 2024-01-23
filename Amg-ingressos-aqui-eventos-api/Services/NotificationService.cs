using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Net.Http.Headers;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Consts;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly MessageReturn _messageReturn;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(EmailTicketDto email)
        {
            _logger.LogInformation(string.Format("Init - Save: {0}", this.GetType().Name));
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                var jsonBody = new StringContent(JsonSerializer.Serialize(email),
                Encoding.UTF8, Application.Json);
                var url = Settings.NotificationServiceApi;
                var uri = Settings.UriNotificationTicket;
                _logger.LogInformation(string.Format("Call PostAsync - Send: {0}", this.GetType().Name));
                await httpClient.PostAsync(url + uri, jsonBody);

                _logger.LogInformation(string.Format("Finished - Save: {0}", this.GetType().Name));
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar notificação.");
                throw;
            }
        }
    }
}