using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IVariantRepository 
    {
        Task<List<Variant>> FindById<T>(string IdVariant);
        Task<object> Save<T>(object variant);
        Task<Variant> Edit<T>(string id, Variant variantObj);
        Task<object> Delete<T>(object IdVariant);
        Task<object> DeleteMany<T>(List<string> variants);
    }
}