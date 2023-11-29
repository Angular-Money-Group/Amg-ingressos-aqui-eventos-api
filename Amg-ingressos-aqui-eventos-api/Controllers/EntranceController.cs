using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/entrance")]
    [Produces("application/json")]
    public class EntranceController : ControllerBase
    {
        private readonly ILogger<EntranceController> _logger;
        private readonly IEntranceService _entranceService;

        public EntranceController(
            ILogger<EntranceController> logger,
            IEntranceService entranceService)
        {
            _logger = logger;
            _entranceService = entranceService;
        }

        /// <summary>
        /// Entrada Ticket
        /// </summary>
        /// <param name="entrance">Objeto dto entrance</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> EntranceAsync([FromBody] EntranceDto entrance)
        {
            try
            {
                if (entrance == null)
                    return StatusCode(400, MessageLogErrors.objectInvalid);
                else
                {
                    if (string.IsNullOrEmpty(entrance.IdTicket))
                        return StatusCode(404, "Ticket não foi informado");
                    if (string.IsNullOrEmpty(entrance.IdColab))
                        return StatusCode(404, "Colaborador não foi informado");
                }

                var result = await _entranceService.EntranceTicket(entrance);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return StatusCode(404, result.Message);
                }

                return Ok(result.Data);
            }
            catch (RuleException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(EntranceAsync),"Entrance"));
                return StatusCode(400, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(EntranceAsync),"Entrance"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(EntranceAsync),"Entrance"));
                return StatusCode(500, string.Format(MessageLogErrors.SaveController,this.GetType().Name, nameof(EntranceAsync),"Entrance"));
            }
        }
    }
}