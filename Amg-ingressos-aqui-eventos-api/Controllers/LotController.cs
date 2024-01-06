using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/lot")]
    public class LotController : ControllerBase
    {
        private readonly ILotService _lotService;
        private readonly ILogger<EventController> _logger;
        
        public LotController(ILotService lotService,
            ILogger<EventController> logger)
        {
            _lotService = lotService;
            _logger = logger;
        }

        /// <summary>
        /// Editar Lot 
        /// </summary>
        /// <param name="id">Id Lot</param>
        /// <param name="lotEdit">dados de lote a serem alterados</param>
        /// <returns>200 Lot Editado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> EditLotAsync([FromRoute] string id, [FromBody] Lot lotEdit)
        {

            var result = await _lotService.EditAsync(id, lotEdit);
            return Ok(result.Data);
        }

        /// <summary>
        /// Editar Lot 
        /// </summary>
        /// <param name="id">Id Lote</param>
        /// <returns>200 Lot deletado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLotAsync([FromRoute] string id)
        {

            var result = await _lotService.DeleteAsync(id);
            return Ok(result.Data);
        }

        /// <summary>
        /// Grava Evento
        /// </summary>
        /// <param name="eventObject">Corpo Evento a ser Gravado</param>
        /// <returns>200 Evento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("managerlots/{id}/{dateManagerLots}")]
        public async Task<IActionResult> ManagerLotsAsync([FromRoute] string id, [FromRoute] DateTime dateManagerLots)
        {
            var result = await _lotService.ManagerLotsAsync(id,dateManagerLots);

            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return StatusCode(500, string.Format(MessageLogErrors.GetController, this.GetType().Name, nameof(ManagerLotsAsync), dateManagerLots));
            }

            return Ok(result.Data);
        }
    }
}