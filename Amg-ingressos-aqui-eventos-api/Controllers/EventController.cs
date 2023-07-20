using Amg_ingressos_aqui_eventos_api.Consts;
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

        public EventController(ILogger<EventController> logger, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        /// <summary>
        /// Busca os eventos destacados ou semanais ou todos
        /// </summary>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetEventsAsync(bool highlights, bool weekly, Pagination paginationOptions)
        {
            try
            {
                var result = await _eventService.GetEventsAsync(highlights, weekly, paginationOptions);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllEventMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllEventMessage);
            }
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
        
        public async Task<IActionResult> FindByIdEventAsync([FromRoute] string id)
        {
            try
            {
                var result = await _eventService.FindByIdAsync(id);
                if (result.Message != null && result.Message.Any())
                {
                    return NotFound(result.Message);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.FindByIdEventMessage, ex);
                return StatusCode(500, MessageLogErrors.FindByIdEventMessage);
            }
        }

        /// <summary>
        /// Busca evento pelo nome
        /// </summary>
        /// <param name="name">Nome desejada do Evento</param>
        /// <returns>200 Evento da busca</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("searchEvents")]
        [Produces("application/json")]
        public async Task<IActionResult> FindEventByNameAsync(string name)
        {
            try
            {
                var result = await _eventService.FindEventByNameAsync(name);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.FindByIdEventMessage, ex);
                return StatusCode(500, MessageLogErrors.FindByIdEventMessage);
            }
        }

        /// <summary>
        /// Busca os eventos do produtor
        /// </summary>
        /// <param name="idOrganizer">Descrição desejada do Evento</param>
        /// <returns>200 Evento da busca</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getEvents/{idOrganizer}")]
        [Produces("application/json")]
        public async Task<IActionResult> FindByOrganizerAsync([FromRoute] string idOrganizer, Pagination paginationOptions)
        {
            try
            {
                var result = await _eventService.FindByOrganizerAsync(idOrganizer, paginationOptions);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.FindByIdEventMessage, ex);
                return StatusCode(500, MessageLogErrors.FindByIdEventMessage);
            }
        }

        /// <summary>
        /// Grava Evento
        /// </summary>
        /// <param name="eventObject">Corpo Evento a ser Gravado</param>
        /// <returns>200 Evento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("createEvent")]
        public async Task<IActionResult> SaveEventAsync([FromBody] Event eventObject)
        {
            try
            {
                var result = await _eventService.SaveAsync(eventObject);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return StatusCode(500, MessageLogErrors.saveEventMessage);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveEventMessage, ex);
                return StatusCode(500, MessageLogErrors.saveEventMessage);
            }
        }

        /// <summary>
        /// Edita destaque evento
        /// </summary>
        /// <param name="id">Corpo Evento a ser Gravado</param>
        /// <returns>200 Evento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("highlightEvent/{id}")]
        public async Task<IActionResult> HighlightEventAsync([FromRoute] string id)
        {
            try
            {
                var result = await _eventService.HighlightEventAsync(id);

                if (result.Message != null && result.Message.Any())
                {

                    if (result.Message == "Maximo de Eventos destacados atingido")
                    {
                        _logger.LogInformation(result.Message);
                        return StatusCode(400, result.Message);
                    }
                    _logger.LogInformation(result.Message);
                    return StatusCode(500, MessageLogErrors.highlightEventmessage);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.highlightEventmessage, ex);
                return StatusCode(500, MessageLogErrors.highlightEventmessage);
            }
        }

        /// <summary>
        /// Editar Evento 
        /// </summary>
        /// <param name="id">Id Evento</param>
        /// <returns>200 Evento Editado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> EditEventAsync([FromRoute] string id, [FromBody] Event eventEdit)
        {
            try
            {
                var result = await _eventService.EditEventsAsync(id, eventEdit);

                return Ok(result.Data);
            }
            catch (DeleteEventException ex)
            {
                _logger.LogInformation(MessageLogErrors.deleteEventMessage, ex);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.deleteEventMessage, ex);
                return StatusCode(500, MessageLogErrors.deleteEventMessage);
            }
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
            try
            {
                var result = await _eventService.DeleteAsync(id);

                return Ok(result.Data); 
            }
            catch (DeleteEventException ex)
            {
                _logger.LogInformation(MessageLogErrors.deleteEventMessage, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.deleteEventMessage, ex);
                return StatusCode(500, MessageLogErrors.deleteEventMessage);
            }
        }
    }
}