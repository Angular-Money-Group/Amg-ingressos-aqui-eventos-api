using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IVariantService
    {
        Task<MessageReturn> SaveAsync(Model.Variant variant);
        Task<MessageReturn> SaveManyAsync(List<Model.Variant> listVariant);
        Task<MessageReturn> EditAsync(List<VariantEditDto> variantEditDto);
        Task<MessageReturn> DeleteAsync(string Idvariant);
        Task<MessageReturn> DeleteManyAsync(List<Model.Querys.Variant> Variants);
    }
}