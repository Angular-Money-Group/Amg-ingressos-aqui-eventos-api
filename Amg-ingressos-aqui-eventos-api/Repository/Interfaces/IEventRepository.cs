using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEventRepository 
    {
        Task<object> Save<T>(object eventComplet);
        Task<T> GetById<T>(string id);
        Task<Event> SetHighlightEvent<T>(string id);
        Task<List<Event>> GetByProducer<T>(string id, Pagination paginationOptions,FilterOptions? filterOptions);
        Task<List<Event>> GetByName<T>(string name);
        Task<object> Delete<T>(object id);
        Task<List<T>> GetAllEvents<T>(Pagination paginationOptions);
        Task<List<GetEventsWithNames>> GetWithUserData<T>();
        Task<List<Model.Querys.GetEventwithTicket.GetEventWithTickets>> GetAllEventsWithTickets(string idEvent,string idOrganizer);
        Task<List<Model.Querys.GetEventTransactions.GetEventTransactions>> GetAllEventsWithTransactions(string idEvent,string idOrganizer);
        Task<Event> Edit<T>(string id, Event eventObj);
        Task<Event> GetByIdVariant<T>(string id);
    }
}