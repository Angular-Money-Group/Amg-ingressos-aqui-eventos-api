using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v2/reports")]
    [Produces("application/json")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;

        private readonly IReportService _reportService;

        public ReportController(ILogger<ReportController> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <param name="idEvent">Id do evento</param>
        /// <param name="idVariant">Id da variante</param>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/{idEvent}/variant/{idVariant}/tickets/details")]
        public IActionResult GetReportEventTicketsDetail([FromRoute] string idEvent, [FromRoute] string idVariant)
        {
            var result = _reportService.GetReportEventTicketsDetail(idEvent, idVariant);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <param name="idEvent">Id do evento</param>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/{idEvent}/tickets/details")]
        public IActionResult GetReportEventTicketsDetails([FromRoute] string idEvent)
        {
            var result = _reportService.GetReportEventTicketsDetails(idEvent);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <param name="idOrganizer">Id do Organizador do evento</param>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/organizer/{idOrganizer}/tickets")]
        public IActionResult GetReportEventTickets([FromRoute] string idOrganizer)
        {
            var result = _reportService.GetReportEventTickets(idOrganizer);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <param name="idEvent">Id do evento</param>
        /// <param name="idVariant">Id da variante</param>
        /// <param name="idOrganizer">Id do usuario organizador do evento</param>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/{idEvent}/transactions/details")]
        public IActionResult GetReportEventTransactionsDetail([FromRoute] string idEvent, [FromQuery] string idOrganizer, [FromQuery] string idVariant)
        {
            var result = _reportService.GetReportEventTransactionsDetail(idEvent, idVariant, idOrganizer);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <param name="idOrganizer">Id do usuario organizador do evento</param>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/organizer/{idOrganizer}/transactions")]
        public IActionResult GetReportEventTransactions([FromRoute] string idOrganizer)
        {
            var result = _reportService.GetReportEventTransactions(idOrganizer);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
        }
    }
}