using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/events")]
    [Produces("application/json")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IEventService _eventService;

        public EventController(
            ILogger<EventController> logger,
            IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        /// <summary>
        /// Busca os eventos
        /// </summary>
        /// <param name="filters"> filtros </param>
        /// <param name="paginationOptions"> paginacao </param>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetEventsAsync(FilterOptions? filters, Pagination paginationOptions)
        {
            if (!ModelState.IsValid)
                throw new RuleException("Dados inválidos");

            var result = await _eventService.GetEventsAsync(filters, paginationOptions);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return StatusCode(500, result.Message);
            }
            if (result.Data.ToString() == string.Empty)
                return NoContent();

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca os eventos
        /// </summary>
        /// <param name="highlights"> eventos em destaque</param>
        /// <param name="weekly"> eventos da semana</param>
        /// <param name="paginationOptions"> paginacao </param>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("Card")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCardEventsAsync(FilterOptions? filters, Pagination paginationOptions)
        {

            if (!ModelState.IsValid)
                throw new RuleException("Dados inválidos");

            var result = await _eventService.GetCardEvents(filters, paginationOptions);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return StatusCode(500, result.Message);
            }
            if (result.Data.ToString() == string.Empty)
                return NoContent();

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca os eventos
        /// </summary>
        /// <param name="highlights"> eventos em destaque</param>
        /// <param name="weekly"> eventos da semana</param>
        /// <param name="paginationOptions"> paginacao </param>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("Card/Highlights")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCardEventsHighligthAsync()
        {

            if (!ModelState.IsValid)
                throw new RuleException("Dados inválidos");

            var result = await _eventService.GetCardEventsHighligth();
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return StatusCode(500, result.Message);
            }
            if (result.Data.ToString() == string.Empty)
                return NoContent();

            return Ok(result.Data);

        }

        /// <summary>
        /// Busca os eventos
        /// </summary>
        /// <param name="paginationOptions"> paginacao </param>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("Card/Weekly")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCardEventsWeeklyAsync()
        {

            if (!ModelState.IsValid)
                throw new RuleException("Dados inválidos");

            var result = await _eventService.GetCardEventsWeekly();
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);

                return StatusCode(500, string.Format(MessageLogErrors.GetController, this.GetType().Name, nameof(GetEventsAsync), "Evento"));
            }
            if (result.Data.ToString() == string.Empty)
                return NoContent();

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca evento pelo ID
        /// </summary>
        /// <param name="id"> id do Evento</param>
        /// <returns>200 Evento da busca</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var result = await _eventService.GetByIdAsync(id);
            if (result.Message != null && result.Message.Any())
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Busca evento pelo nome
        /// </summary>
        /// <param name="name">Nome desejada do Evento</param>
        /// <returns>200 Evento da busca</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("search")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            FilterOptions filters = new FilterOptions() { Name = name };
            var result = await _eventService.GetEventsAsync(filters, null);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NoContent();
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Grava Evento
        /// </summary>
        /// <param name="eventObject">Corpo Evento a ser Gravado</param>
        /// <returns>200 Evento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveEventAsync([FromBody] EventCompletDto eventObject)
        {
            var result = await _eventService.SaveAsync(eventObject);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return StatusCode(500, string.Format(MessageLogErrors.GetController, this.GetType().Name, nameof(SaveEventAsync), "Evento"));
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Edita destaque evento
        /// </summary>
        /// <param name="id">id evento</param>
        /// <returns>200 Evento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("highlightEvent/{id}")]
        public async Task<IActionResult> SetHighlightEventAsync([FromRoute] string id)
        {
            var result = await _eventService.SetHighlightEventAsync(id);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return StatusCode(204, string.Format(MessageLogErrors.EditController, this.GetType().Name, nameof(SetHighlightEventAsync), "Evento"));
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Editar Evento 
        /// </summary>
        /// <param name="id">Id Evento</param>
        /// <param name="eventEdit">Ojbeto para qual vai ser alterado o evento</param>
        /// <returns>200 Evento Editado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> EditEventAsync([FromRoute] string id, [FromBody] EventEditDto eventEdit)
        {

            var result = await _eventService.EditEventsAsync(id, eventEdit);
            return Ok(result.Data);
        }

        /// <summary>
        /// Delete Evento 
        /// </summary>
        /// <param name="id">Id Evento</param>
        /// <returns>200 Evento deletado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteEventAsync(string id)
        {
            var result = await _eventService.DeleteAsync(id);
            return Ok(result.Data);
        }
    }
}