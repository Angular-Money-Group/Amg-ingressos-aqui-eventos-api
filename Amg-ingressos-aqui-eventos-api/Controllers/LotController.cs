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
        /// <param name="lotEdit">dados de lote a serem alterados</param>
        /// <returns>200 Lot Editado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> EditLotAsync([FromRoute] string id, [FromBody]Lot lotEdit)
        {
            try
            {
                var result = await _LotService.EditAsync(id, lotEdit);
                return Ok(result.Data);
            }
            catch (DeleteException ex)
            {
                _logger.LogInformation(ex, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditLotAsync),"Lote"));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditLotAsync),"Lote"));
                return StatusCode(500, string.Format(MessageLogErrors.EditController,this.GetType().Name, nameof(EditLotAsync),"Lote"));
            }
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
            try
            {
                var result = await _LotService.DeleteAsync(id);
                return Ok(result.Data);
            }
            catch (DeleteException ex)
            {
                _logger.LogInformation(ex, string.Format(MessageLogErrors.DeleteController,this.GetType().Name, nameof(DeleteLotAsync),"Lote"));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.DeleteController,this.GetType().Name, nameof(DeleteLotAsync),"Lote"));
                return StatusCode(500, string.Format(MessageLogErrors.DeleteController,this.GetType().Name, nameof(DeleteLotAsync),"Lote"));
            }
        }
    }
}