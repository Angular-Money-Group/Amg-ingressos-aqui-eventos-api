using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ILotRepository 
    {
        Task<object> Save<T>(object lot);
        Task<object> Delete<T>(object lot);
    }
}