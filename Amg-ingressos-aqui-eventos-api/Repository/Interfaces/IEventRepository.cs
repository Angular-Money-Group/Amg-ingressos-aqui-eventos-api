using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEventRepository 
    {
        Task<object> Save<T>(object eventComplet);
        Task<List<Event>> GetAllEvents<T>(Pagination paginationOptions);
        Task<List<Event>> GetWeeklyEvents<T>(Pagination paginationOptions);
        Task<List<Event>> GetHighlightedEvents<T>(Pagination paginationOptions);
        Task<List<GetEvents>> FindById<T>(object id);
        Task<List<Event>> FindByProducer<T>(string id, Pagination paginationOptions);
        Task<List<Event>> FindByName<T>(string name);
        Task<Event> Edit<T>(string id, Event eventEdit);
        Task<Event> SetHighlightEvent<T>(string id);
        Task<object> Delete<T>(object id);
    }
}