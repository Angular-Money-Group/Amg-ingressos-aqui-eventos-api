using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
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

        public EntranceController(ILogger<EntranceController> logger, IEntranceService entranceService)
        {
            _logger = logger;
            _entranceService = entranceService;
        }

        /// <summary>
        /// Entrada Ticket
        /// </summary>
        /// <param name="id">Id do ticket</param>
        /// <param name="ticket">Objeto do ticket a ser alterado</param>
        /// <returns>200 Edita os dados do Ticket</returns>
        /// <returns>204 Nenhum ticket encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> entranceAsync([FromBody] EntranceDto entrance)
        {
            try
            {
                if (entrance == null)
                {
                    return StatusCode(400, MessageLogErrors.objectInvalid);
                }
                else
                {
                    if (string.IsNullOrEmpty(entrance.IdTicket))
                    {
                        return StatusCode(404, "Ticket não foi informado");
                    }

                    if (string.IsNullOrEmpty(entrance.IdColab))
                    {
                        return StatusCode(404, "Colaborador não foi informado");
                    }
                }

                var result = await _entranceService.entranceTicket(entrance);

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return StatusCode( Convert.ToInt32(result.Data), result.Message);
                }

                return Ok(result.Data);
            }
            //catch (NotModificateTicketsExeption ex)
            //{
            //    _logger.LogError(MessageLogErrors.NotModificateTickets, ex);
            //    return StatusCode(444, MessageLogErrors.NotModificateTickets);
            //}
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.entranceTicket, ex.Message);
                return StatusCode(500, MessageLogErrors.entranceTicket + ex.Message);
            }
        }



    }
}
