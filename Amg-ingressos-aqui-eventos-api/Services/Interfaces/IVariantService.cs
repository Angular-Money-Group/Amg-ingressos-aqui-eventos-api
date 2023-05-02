using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IVariantService
    {
        Task<MessageReturn> SaveAsync(Variant variant);
    }
}