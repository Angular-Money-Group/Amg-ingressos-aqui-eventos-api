using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IVariantRepository 
    {
        Task<object> Save<T>(object variant);
    }
}