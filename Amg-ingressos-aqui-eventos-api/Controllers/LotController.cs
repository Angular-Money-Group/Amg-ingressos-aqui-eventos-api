using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/lot")]
    public class LotController : ControllerBase
    {
        private readonly ILogger<LotController> _logger;
        private readonly ILotService _LotService;

        public LotController(ILogger<LotController> logger, ILotService LotService)
        {
            _logger = logger;
            _LotService = LotService;
        }

        /// <summary>
        /// Editar Lot 
        /// </summary>
        /// <param name="id">Id Lot</param>
        /// <returns>200 Lot Editado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> EditLotAsync([FromRoute] string id, [FromBody] Lot lotEdit)
        {
            try
            {
                var result = await _LotService.EditAsync(id, lotEdit);

                return Ok(result.Data);
            }
            catch (DeleteEventException ex)
            {
                _logger.LogInformation(MessageLogErrors.saveEventMessage, ex);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveEventMessage, ex);
                return StatusCode(500, MessageLogErrors.saveEventMessage);
            }
        }
    }
}