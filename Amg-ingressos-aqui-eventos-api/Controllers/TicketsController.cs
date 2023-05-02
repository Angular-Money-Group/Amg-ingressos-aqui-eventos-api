using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;

        public TicketController(ILogger<TicketController> logger, IEventService eventService, ITicketService ticketService)
        {
            _logger = logger;
            _eventService = eventService;
            _ticketService = ticketService;
        }

        /// <summary>
        /// Busca todos os ingressos do usuário
        /// </summary>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getTickets")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserTicketsAsync(string id)
        {
            try
            {
                var result = await _ticketService.GetUserTicketsAsync(id); 
                
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data as List<Ticket>);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.FindByUserIdTicketMessage, ex.Message);
                return StatusCode(500, MessageLogErrors.FindByUserIdTicketMessage + ex.Message);
            }
        }
}
}