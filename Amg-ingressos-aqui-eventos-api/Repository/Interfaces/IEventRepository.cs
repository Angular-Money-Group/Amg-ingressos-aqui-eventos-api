using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEventRepository 
    {
        Task<object> Save<T>(object eventComplet);
        Task<object> FindById<T>(object id);
        Task<List<Event>> FindByProducer<T>(string id, Pagination paginationOptions);
        Task<List<Event>> FindByName<T>(string name);
        Task<object> Delete<T>(object id);
        Task<List<Event>> GetAllEvents<T>(Pagination paginationOptions);
        Task<List<Event>> GetWeeklyEvents<T>(Pagination paginationOptions);
        Task<List<Event>> GetHighlightedEvents<T>(Pagination paginationOptions);
    }
}