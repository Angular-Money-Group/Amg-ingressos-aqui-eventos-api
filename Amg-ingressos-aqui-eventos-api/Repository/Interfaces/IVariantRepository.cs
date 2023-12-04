using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IVariantRepository 
    {
        Task<List<Variant>> FindById<T>(string id);
        Task<object> Save<T>(object variant);
        Task<object> SaveMany<T>(List<Variant> listVariant);
        Task<Variant> Edit<T>(string id, Variant variant);
        Task<object> Delete<T>(object id);
        Task<object> DeleteMany<T>(List<string> listVariant);
    }
}