using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/variant")]
    [Produces("application/json")]
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
        public async Task<IActionResult> EditAsync([FromBody] List<VariantEditDto> listVariant)
        {
            try
            {
                var result = await _variantService.EditAsync(listVariant);
                return Ok(result.Data);
            }
            catch (EditException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.EditController, this.GetType().Name, nameof(EditAsync), "Variante(s)"));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.EditController, this.GetType().Name, nameof(EditAsync), "Variante(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.EditController, this.GetType().Name, nameof(EditAsync), "Variante(s)"));
            }
        }

        /// <summary>
        /// Editar Variant 
        /// </summary>
        /// <param name="variants">lista de variantes a inserir massivamente</param>
        /// <returns>200 mensagem sucesso</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("save/variants")]
        [HttpPost]
        public async Task<IActionResult> SaveVariantAsync([FromBody] List<VariantWithLotDto> variants)
        {
            try
            {
                var result = await _variantService.SaveManyAsync(variants);
                return Ok(result.Data);
            }
            catch (SaveException)
            {
                _logger.LogInformation(string.Format(MessageLogErrors.SaveController, this.GetType().Name, nameof(SaveVariantAsync), "Variante(s)"));
                return StatusCode(404, string.Format(MessageLogErrors.SaveController, this.GetType().Name, nameof(SaveVariantAsync), "Variante(s)"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.SaveController, this.GetType().Name, nameof(SaveVariantAsync), "Variante(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.SaveController, this.GetType().Name, nameof(SaveVariantAsync), "Variante(s)"));
            }
        }

        /// <summary>
        /// Editar Variant 
        /// </summary>
        /// <param name="id">Id Variant</param>
        /// <returns>200 mensagem sucesso ao deletar</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            try
            {
                var result = await _variantService.DeleteAsync(id);
                return Ok(result.Data);
            }
            catch (DeleteException ex)
            {
                _logger.LogInformation(ex,string.Format(MessageLogErrors.DeleteController, this.GetType().Name, nameof(DeleteAsync), "Variante(s)"));
                return StatusCode(404, string.Format(MessageLogErrors.SaveController, this.GetType().Name, nameof(SaveVariantAsync), "Variante(s)"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.DeleteController, this.GetType().Name, nameof(DeleteAsync), "Variante(s)"));
                return StatusCode(500, string.Format(MessageLogErrors.DeleteController, this.GetType().Name, nameof(DeleteAsync), "Variante(s)"));
            }
        }
    }
}