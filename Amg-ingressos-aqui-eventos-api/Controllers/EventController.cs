using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/eventos")]
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
        /// Busca todos os eventos
        /// </summary>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getAllEvents")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllEventsAsync()
        {
            try
            {
                var result = await _eventService.GetAllEventsAsync();
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
        /// Busca eventos da semana
        /// </summary>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getWeeklyEvents")]
        [Produces("application/json")]
        public async Task<IActionResult> GetWeeklyEventsAsync(Pagination paginationOptions)
        {
            try
            {
                var result = await _eventService.GetWeeklyEventsAsync(paginationOptions);
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
        [Route("getEventById")]
        [Produces("application/json")]
        public async Task<IActionResult> FindByIdEventAsync(string id)
        {
            try
            {
                var result = await _eventService.FindByIdAsync(id);
                if(result.Message!= null && result.Message.Any()){
                    return NotFound(result.Message);
                }
                return Ok(result.Data);
            }
            catch (FindByIdEventException ex)
            {
                _logger.LogInformation(MessageLogErrors.FindByIdEventMessage, ex);
                return NoContent();
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

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveEventMessage, ex);
                return StatusCode(500, MessageLogErrors.saveEventMessage);
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
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.deleteEventMessage, ex);
                return StatusCode(500, MessageLogErrors.deleteEventMessage);
            }
        }
    }
}