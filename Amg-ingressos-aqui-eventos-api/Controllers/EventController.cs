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
        /// <param name="highlights"> eventos em destaque</param>
        /// <param name="weekly"> eventos da semana</param>
        /// <param name="paginationOptions"> paginacao </param>
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
                    
                    return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetEventsAsync),"Evento"));
                }
                if(result.Data.ToString() == string.Empty)
                    return NoContent();

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetEventsAsync),"Eventos"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetEventsAsync),"Eventos"));
            }
        }

        /// <summary>
        /// Busca os eventos com organizador
        /// </summary>
        /// <returns>200 Lista de todos eventos</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getAllAdmin")]
        [Produces("application/json")]
        public async Task<IActionResult> GetWithUserData()
        {
            try
            {
                var result = await _eventService.GetWithUserData();
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetWithUserData),"Eventos"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetWithUserData),"Eventos"));
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
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            try
            {
                var result = await _eventService.GetByIdAsync(id);
                if (result.Message != null && result.Message.Any())
                {
                    return NotFound(result.Message);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByIdAsync),"Evento"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByIdAsync),"Evento"));
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
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            try
            {
                var result = await _eventService.GetByNameAsync(name);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByNameAsync),"Evento"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByNameAsync),"Evento"));
            }
        }

        /// <summary>
        /// Busca os eventos do organizador
        /// </summary>
        /// <param name="idOrganizer">Descrição desejada do Evento</param>
        /// <returns>200 Evento da busca</returns>
        /// <returns>204 Nenhum evento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getEvents/{idOrganizer}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByOrganizerAsync([FromRoute] string idOrganizer, Pagination paginationOptions, FilterOptions? filter)
        {
            try
            {
                var result = await _eventService.GetByOrganizerAsync(idOrganizer, paginationOptions, filter);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByOrganizerAsync),"Evento"));
                return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(GetByOrganizerAsync),"Evento"));
            }
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
            try
            {
                var result = await _eventService.SaveAsync(eventObject);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return StatusCode(500, string.Format(MessageLogErrors.GetController,this.GetType().Name, nameof(SaveEventAsync),"Evento"));
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(SaveEventAsync),"Evento"));
                return StatusCode(500, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(SaveEventAsync),"Evento"));
            }
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
            try
            {
                var result = await _eventService.SetHighlightEventAsync(id);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return StatusCode(204, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(SetHighlightEventAsync),"Evento"));
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(SetHighlightEventAsync),"Evento"));
                return StatusCode(500, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(SetHighlightEventAsync),"Evento"));
            }
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
            try
            {
                var result = await _eventService.EditEventsAsync(id, eventEdit);
                return Ok(result.Data);
            }
            catch (EditException ex)
            {
                _logger.LogInformation(ex, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditEventAsync),"Evento"));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditEventAsync),"Evento"));
                return StatusCode(500, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditEventAsync),"Evento"));
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
            catch (DeleteException ex)
            {
                _logger.LogInformation(ex, string.Format(MessageLogErrors.DeleteController,this.GetType().Name, nameof(DeleteEventAsync),"Evento"));
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.DeleteController,this.GetType().Name, nameof(DeleteEventAsync),"Evento"));
                return StatusCode(500, string.Format(MessageLogErrors.DeleteController,this.GetType().Name, nameof(DeleteEventAsync),"Evento"));
            }
        }
    }
}