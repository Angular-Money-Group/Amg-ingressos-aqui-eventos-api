using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IVariantService
    {
        Task<MessageReturn> SaveAsync(Variant variant);
        Task<MessageReturn> EditAsync(string id, Variant variant);
        Task<MessageReturn> DeleteAsync(string Idvariant);
        Task<MessageReturn> DeleteManyAsync(List<Model.Querys.Variant> Variants);
    }
}