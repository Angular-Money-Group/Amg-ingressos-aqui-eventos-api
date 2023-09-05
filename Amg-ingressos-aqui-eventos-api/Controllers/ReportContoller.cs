using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/{idEvent}/variant/{idVariant}/tickets/details")]
        public async Task<IActionResult> GetReportEventTicketsDetail([FromRoute] string idEvent, [FromRoute] string idVariant)
        {
            try
            {
                var result = await _reportService.GetReportEventTicketsDetail(idEvent, idVariant);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.findTicketByUser, ex.Message);
                return StatusCode(500, MessageLogErrors.findTicketByUser + ex.Message);
            }
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/organizer/{idOrganizer}/tickets")]
        public async Task<IActionResult> GetReportEventTickets([FromRoute] string idOrganizer)
        {
            try
            {
                var result = await _reportService.GetReportEventTickets(idOrganizer);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.findTicketByUser, ex.Message);
                return StatusCode(500, MessageLogErrors.findTicketByUser + ex.Message);
            }
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/{idEvent}/transactions/details")]
        public async Task<IActionResult> GetReportEventTransactionsDetail([FromRoute] string idEvent, [FromQuery] string idOrganizer, [FromQuery] string idVariant)
        {
            try
            {
                var result = await _reportService.GetReportEventTransactionsDetail(idEvent, idVariant, idOrganizer);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.findTicketByUser, ex.Message);
                return StatusCode(500, MessageLogErrors.findTicketByUser + ex.Message);
            }
        }

        /// <summary>
        /// Relatorio de evento com variante e tickets detalhado
        /// </summary>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/organizer/{idOrganizer}/transactions")]
        public async Task<IActionResult> GetReportEventTransactions([FromRoute] string idOrganizer)
        {
            try
            {
                var result = await _reportService.GetReportEventTransactions(idOrganizer);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.findTicketByUser, ex.Message);
                return StatusCode(500, MessageLogErrors.findTicketByUser + ex.Message);
            }
        }
    }
}