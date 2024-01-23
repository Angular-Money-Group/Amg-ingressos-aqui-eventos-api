using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IVariantRepository : ICrudRepository<Variant>
    {
        Task<List<Variant>> SaveMany(List<Variant> listVariant);
        Task<bool> DeleteMany(List<string> listVariant);
        Task<bool> ChangeStatusVariant(string id, int statusVariant);
    }
}