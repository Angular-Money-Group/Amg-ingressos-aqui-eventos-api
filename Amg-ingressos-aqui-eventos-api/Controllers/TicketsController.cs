using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/tickets")]
    [Produces("application/json")]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly ITicketService _ticketService;

        public TicketController(ILogger<TicketController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        /// <summary>
        /// Busca todos os ingressos do usuário
        /// </summary>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("user/{idUser}")]
        public async Task<IActionResult> GetTicketByUser([FromRoute] string idUser)
        {
            try
            {
                var result = await _ticketService.GetTicketByUser(idUser);

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
        /// Busca todos os ingressos do usuário
        /// </summary>
        /// <param name="id">Id do lote</param>
        /// <returns>200 Retorna a quantidade de tickets disponiveis</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("lote/{idLote}")]
        public async Task<IActionResult> GetTicketsRemainingByLot([FromRoute] string idLote)
        {
            try
            {
                var result = await _ticketService.GetTicketsRemainingByLot(idLote);

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
        /// Busca todos os ingressos do usuário
        /// </summary>
        /// <param name="id">Id do lote</param>
        /// <returns>200 Retorna a quantidade de tickets disponiveis</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetTicketById(id);

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
        /// Busca todos os ingressos do usuário
        /// </summary>
        /// <param name="id">Id do lote</param>
        /// <returns>200 Retorna a quantidade de tickets disponiveis</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}/datauser")]
        public async Task<IActionResult> GetTicketByIdDataUser([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetTicketByIdDataUser(id);

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
        /// Busca todos os ingressos do usuário
        /// </summary>
        /// <param name="id">Id do lote</param>
        /// <returns>200 Retorna a quantidade de tickets disponiveis</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}/dataevent")]
        public async Task<IActionResult> GetTicketByIdDataEvent([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetTicketByIdDataEvent(id);

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
        /// Edita os dados do Ticket
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <param name="ticket">Objeto do ticket a ser alterado</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTicketsAsync(
            [FromRoute] string id,
            [FromBody] Ticket ticket
        )
        {
            try
            {
                var result = await _ticketService.UpdateTicketsAsync(id, ticket);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (NotModificateTicketsExeption ex)
            {
                _logger.LogError(MessageLogErrors.NotModificateTickets, ex);
                return StatusCode(444, MessageLogErrors.NotModificateTickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.UpdateTickets, ex.Message);
                return StatusCode(500, MessageLogErrors.UpdateTickets + ex.Message);
            }
        }

        /// <summary>
        /// Edita os dados do Ticket
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <param name="ticket">Objeto do ticket a ser alterado</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveTicketsAsync([FromBody] Ticket ticket)
        {
            try
            {
                var result = await _ticketService.SaveAsync(ticket);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Message);
            }
            catch (NotModificateTicketsExeption ex)
            {
                _logger.LogError(MessageLogErrors.NotModificateTickets, ex);
                return StatusCode(444, MessageLogErrors.NotModificateTickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.UpdateTickets, ex.Message);
                return StatusCode(500, MessageLogErrors.UpdateTickets + ex.Message);
            }
        }

        /// <summary>
        /// Edita os dados do Ticket
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <param name="ticket">Objeto do ticket a ser alterado</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("sendCourtesy")]
        public async Task<IActionResult> SendCourtesyTickets(
            [FromBody] GenerateCourtesyTicketDto courtesyTicketDto
        )
        {
            var _messageReturn = new MessageReturn();

            try
            {
                var result = _ticketService.SendCourtesyTickets(courtesyTicketDto);

                _messageReturn.Data = "Enviando courtesias";
                _messageReturn.Message = null;
                return Ok(_messageReturn);
            }
            catch (NotModificateTicketsExeption ex)
            {
                _logger.LogError(MessageLogErrors.NotModificateTickets, ex);
                return StatusCode(444, MessageLogErrors.NotModificateTickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.UpdateTickets, ex.Message);
                return StatusCode(500, MessageLogErrors.UpdateTickets + ex.Message);
            }
        }

        /// <summary>
        /// Edita os dados do Ticket
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <param name="ticket">Objeto do ticket a ser alterado</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("reSendCourtesy")]
        public async Task<IActionResult> ReSendCourtesyTickets(string rowTicketId, string variantId)
        {
            var _messageReturn = new MessageReturn();

            try
            {
                var result = _ticketService.ReSendCourtesyTickets(rowTicketId, variantId);

                _messageReturn.Data = "Enviando courtesias";
                _messageReturn.Message = null;
                return Ok(_messageReturn);
            }
            catch (NotModificateTicketsExeption ex)
            {
                _logger.LogError(MessageLogErrors.NotModificateTickets, ex);
                return StatusCode(444, MessageLogErrors.NotModificateTickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.UpdateTickets, ex.Message);
                return StatusCode(500, MessageLogErrors.UpdateTickets + ex.Message);
            }
        }

        /// <summary>
        /// Edita os dados do Ticket
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("courtesy/{id}")]
        public async Task<IActionResult> GetCourtesyStatus([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetCourtesyStatus(id);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.UpdateTickets, ex.Message);
                return StatusCode(500, MessageLogErrors.UpdateTickets + ex.Message);
            }
        }
    }
}
