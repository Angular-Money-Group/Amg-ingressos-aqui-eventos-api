using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
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

        public TicketController(){}

        public TicketController(ILogger<TicketController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        /// <summary>
        /// Busca todos os ingressos do usuário
        /// </summary>
        /// <param name="idUser">Id do usuario do ticket</param>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("user/{idUser}")]
        public async Task<IActionResult> GetByUser([FromRoute] string idUser)
        {
            try
            {
                var result = await _ticketService.GetByUser(idUser);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByUserAndEvent),"Ticket"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByUserAndEvent),"Ticket"));
            }
        }
        /// <summary>
        /// Busca todos os ingressos do usuário para o evento
        /// </summary>
        /// <param name="idUser">Id do usuario do ticket</param>
        /// <param name="idEvent">Id do evento</param>
        /// <returns>200 Lista de todos os tickets</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("user/{idUser}/event/{idEvent}")]
        public async Task<IActionResult> GetByUserAndEvent([FromRoute] string idUser,[FromRoute] string idEvent)
        {
            try
            {
                var result = await _ticketService.GetByUserAndEvent(idUser,idEvent);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByUserAndEvent),"Ticket"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByUserAndEvent),"Ticket"));
            }
        }

        /// <summary>
        /// Busca todos os ingresso restantes do lote
        /// </summary>
        /// <param name="idLote">Id do lote</param>
        /// <returns>200 Retorna a quantidade de tickets disponiveis</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("lote/{idLote}")]
        public async Task<IActionResult> GetRemainingByLot([FromRoute] string idLote)
        {
            try
            {
                var result = await _ticketService.GetRemainingByLot(idLote);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetRemainingByLot),"Tickets"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetRemainingByLot),"Tickets"));
            }
        }

        /// <summary>
        /// Busca ingresso pelo id
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <returns>200 Retorna ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetById(id);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetById),"Ticket"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetById),"Ticket"));
            }
        }

        /// <summary>
        /// Busca ingresso por id com dados de usuario
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <returns>200 Retorna ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}/datauser")]
        public async Task<IActionResult> GetByIdWithDataUser([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetByIdWithDataUser(id);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByIdWithDataUser),"Ticket(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByIdWithDataUser),"Ticket(s)"));
            }
        }

        /// <summary>
        /// Busca ingresso com dados de evento
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <returns>200 Retorna ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}/dataevent")]
        public async Task<IActionResult> GetByIdWithDataEvent([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetByIdWithDataEvent(id);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByIdWithDataEvent),"Ticket(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByIdWithDataEvent),"Ticket(s)"));
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
        public async Task<IActionResult> EditAsync(
            [FromRoute] string id,
            [FromBody] Ticket ticket
        )
        {
            try
            {
                var result = await _ticketService.EditAsync(id, ticket);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditAsync),"Ticket(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditAsync),"Ticket(s)"));
            }
        }

        /// <summary>
        /// Cria novo Ticket
        /// </summary>
        /// <param name="ticket">Objeto do ticket a ser alterado</param>
        /// <returns>200 devolve mensage de sucesso</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] Ticket ticket)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(SaveAsync),"Ticket(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(SaveAsync),"Ticket(s)"));
            }
        }

        /// <summary>
        /// cria e envia cortesias
        /// </summary>
        /// <param name="courtesyTicketDto">dados de cortesia</param>
        /// <returns>200 retorna mensagem de sucesso cortesia</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("sendCourtesy")]
        public IActionResult SendCourtesyTickets(
            [FromBody] GenerateCourtesyTicketDto courtesyTicketDto
        )
        {
            try
            {
                var _messageReturn = _ticketService.SendCourtesyTickets(courtesyTicketDto);
                _messageReturn.Data = "Enviando courtesias";
                return Ok(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.SendController, this.GetType().Name, nameof(SendCourtesyTickets), "Cortesia(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.SendController, this.GetType().Name, nameof(SendCourtesyTickets), "Cortesia(s)"));
            }
        }

        /// <summary>
        /// Reenvia cortesias
        /// </summary>
        /// <param name="rowTicketId">Id de cortesia enviada</param>
        /// <param name="variantId">Id da variante</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("reSendCourtesy")]
        public IActionResult ReSendCourtesyTickets(string rowTicketId, string variantId)
        {
            try
            {
                var result = _ticketService.ReSendCourtesyTickets(rowTicketId, variantId);
                result.Data = "Enviando courtesias";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.SendController, this.GetType().Name, nameof(ReSendCourtesyTickets), "Cortesia(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.SendController, this.GetType().Name, nameof(ReSendCourtesyTickets), "Cortesia(s)"));
            }
        }

        /// <summary>
        /// Edita os dados do Ticket
        /// </summary>
        /// <param name="id">id cortesia</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("courtesy/{id}")]
        public async Task<IActionResult> GetCourtesyStatusById([FromRoute] string id)
        {
            try
            {
                var result = await _ticketService.GetCourtesyStatusById(id);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController, this.GetType().Name, nameof(GetCourtesyStatusById), "Cortesia(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController, this.GetType().Name, nameof(GetCourtesyStatusById), "Cortesia(s)"));
            }
        }
    }
}