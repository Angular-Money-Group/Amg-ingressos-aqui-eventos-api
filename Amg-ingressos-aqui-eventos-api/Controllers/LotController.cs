using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/lots")]
    public class LotController : ControllerBase
    {
        private readonly ILogger<LotController> _logger;
        private readonly ILotService _lotService;

        public LotController(ILogger<LotController> logger, ILotService lotService)
        {
            _logger = logger;
            _lotService = lotService;
        }

        /// <summary>
        /// Verifica se os Lots serão vendidos
        /// </summary>
        /// <returns>200 Retorna Lista valores booleano para a exibição ou não dos lotes</returns>
        /// <returns>404 Nenhum Lote encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> VerifyLotsAvaibleAsync(string idVariant)
        {
             try
            {
                var result = await _lotService.VerifyLotsAvaibleAsync(idVariant);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return BadRequest(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllEventMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllEventMessage);
            }
        }
    }
}