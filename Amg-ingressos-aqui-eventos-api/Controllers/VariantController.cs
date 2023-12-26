using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_eventos_api.Controllers
{
    [Route("v1/variant")]
    [Produces("application/json")]
    public class VariantController : ControllerBase
    {
        private readonly IVariantService _variantService;

        public VariantController(IVariantService variantService)
        {
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
            var result = await _variantService.EditAsync(listVariant);
            return Ok(result.Data);
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
            var result = await _variantService.SaveManyAsync(variants);
            return Ok(result.Data);
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
            var result = await _variantService.DeleteAsync(id);
            return Ok(result.Data);
        }
    }
}