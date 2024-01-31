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
        
        public LotController(ILotService lotService)
        {
            _lotService = lotService;
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
    }
}