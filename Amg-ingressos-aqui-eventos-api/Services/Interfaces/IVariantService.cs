using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IVariantService
    {
        Task<MessageReturn> SaveAsync(VariantWithLotDto variant);
        Task<MessageReturn> SaveManyAsync(List<VariantWithLotDto> listVariant);
        Task<MessageReturn> EditAsync(List<VariantEditDto> listVariant);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> DeleteManyAsync(List<VariantWithLotDto> listVariant);
    }
}