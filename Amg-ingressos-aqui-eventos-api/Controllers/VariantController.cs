using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/variant")]
    public class VariantController : ControllerBase
    {
        private readonly ILogger<VariantController> _logger;
        private readonly IVariantService _variantService;

        public VariantController(ILogger<VariantController> logger, IVariantService variantService)
        {
            _logger = logger;
            _variantService = variantService;
        }

        /// <summary>
        /// Editar Variant 
        /// </summary>
        /// <param name="id">Id Variant</param>
        /// <returns>200 Variant Editado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("edit/variants")]
        [HttpPatch]
        public async Task<IActionResult> EditVariantAsync([FromBody] List<VariantEditDto> variant)
        {
            try
            {
                var result = await _variantService.EditAsync(variant);

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
        /// Editar Variant 
        /// </summary>
        /// <param name="id">Id Variant</param>
        /// <returns>200 Variant Editado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("save/variants")]
        [HttpPost]
        public async Task<IActionResult> SaveVariantAsync([FromBody] List<Model.Variant> variants)
        {
            try
            {
                var result = await _variantService.SaveManyAsync(variants);

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