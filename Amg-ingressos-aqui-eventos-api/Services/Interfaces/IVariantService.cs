using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IVariantService
    {
        Task<MessageReturn> SaveAsync(Variant variant);
        Task<MessageReturn> SaveManyAsync(List<Variant> listVariant);
        Task<MessageReturn> EditAsync(List<VariantEditDto> listVariant);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> DeleteManyAsync(List<Variant> listVariant);
    }
}