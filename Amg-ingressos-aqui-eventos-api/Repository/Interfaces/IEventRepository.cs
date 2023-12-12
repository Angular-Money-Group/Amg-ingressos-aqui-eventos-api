using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEventRepository
    {
        Task<object> Save<T>(object eventComplet);
        Task<T> GetById<T>(string id);
        Task<Event> SetHighlightEvent<T>(string id);
        Task<List<Event>> GetByProducer<T>(string id, Pagination paginationOptions, FilterOptions? filterOptions);
        Task<List<Event>> GetByName<T>(string name);
        Task<object> Delete<T>(object id);
        Task<List<T>> GetAllEvents<T>(Pagination paginationOptions);
        Task<List<T>> GetAllEventsWithTickets<T>(string idEvent, string idOrganizer);
        Task<List<T>> GetAllEventsWithTransactions<T>(string idEvent, string idOrganizer);
        Task<Event> Edit<T>(string id, Event eventObj);
        Task<Event> GetByIdVariant<T>(string id);
    }
}