using Amg_ingressos_aqui_eventos_api.Dto;
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
            var result = await _ticketService.GetByUser(idUser);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
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
        public async Task<IActionResult> GetByUserAndEvent([FromRoute] string idUser, [FromRoute] string idEvent)
        {
            var result = await _ticketService.GetByUserAndEvent(idUser, idEvent);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Busca todos os ingresso restantes do lote
        /// </summary>
        /// <param name="idLote">Id do lote</param>
        /// <returns>200 Retorna tickets pelo lote</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("lote/{idLote}")]
        public async Task<IActionResult> GetTicketsbyLot([FromRoute] string idLote)
        {
            var result = await _ticketService.GetTicketsByLot(idLote);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca todos os ingresso restantes do lote
        /// </summary>
        /// <param name="idLote">Id do lote</param>
        /// <returns>200 Retorna tickets pelo lote</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("countTickets/lote/{idLote}")]
        public async Task<IActionResult> GetRemainingByLot([FromRoute] string idLote)
        {
            var result = await _ticketService.GetRemainingByLot(idLote);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }

            return Ok(result.Data);
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
            var result = await _ticketService.GetById(id);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }

            return Ok(result.Data);
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
            var result = await _ticketService.GetByIdWithDataUser(id);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }

            return Ok(result.Data);
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
            var result = await _ticketService.GetByIdWithDataEvent(id);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }

            return Ok(result.Data);
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
        public async Task<IActionResult> EditAsync([FromRoute] string id, [FromBody] Ticket ticket)
        {
            var result = await _ticketService.EditAsync(id, ticket);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }

            return Ok(result.Data);
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
            var result = await _ticketService.SaveAsync(ticket);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }

            return Ok(result.Message);
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
        public IActionResult SendCourtesyTickets([FromBody] CourtesyTicketDto courtesyTicketDto)
        {
            var _messageReturn = _ticketService.SendCourtesyTickets(courtesyTicketDto);
            _messageReturn.Data = "Enviando courtesias";
            return Ok(_messageReturn);
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
            var result = _ticketService.ReSendCourtesyTickets(rowTicketId, variantId);
            result.Data = "Enviando courtesias";
            return Ok(result);
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
            var result = await _ticketService.GetCourtesyStatusById(id);
            return Ok(result.Data);
        }
    }
}