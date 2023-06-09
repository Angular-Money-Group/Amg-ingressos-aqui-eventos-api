using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEventRepository 
    {
        Task<object> Save<T>(object eventComplet);
        Task<object> FindById<T>(object id);
        Task<object> Delete<T>(object id);
        Task<IEnumerable<object>> GetAllEvents<T>();
    }
}