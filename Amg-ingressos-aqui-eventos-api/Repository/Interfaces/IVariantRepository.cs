using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IVariantRepository : ICrudRepository<Variant>
    {
        Task<bool> SaveMany(List<Variant> listVariant);
        Task<bool> DeleteMany(List<string> listVariant);
    }
}