using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEventRepository 
    {
        Task<object> Save<T>(object eventComplet);
        Task<object> FindById<T>(object id);
        Task<Event> SetHighlightEvent<T>(string id);
        Task<List<Event>> FindByProducer<T>(string id, Pagination paginationOptions,FilterOptions? filter);
        Task<List<Event>> FindByName<T>(string name);
        Task<object> Delete<T>(object id);
        Task<List<GetEventsWithNames>> GetAllEvents<T>(Pagination paginationOptions);
        Task<List<GetEventsWithNames>> GetAllEventsAdmin<T>();
        Task<List<Model.Querys.GetEventwithTicket.GetEventWitTickets>> GetAllEventsWithTickets(string idEvent,string idOrganizer);
        Task<List<Model.Querys.GetEventTransactions.GetEventTransactions>> GetAllEventsWithTransactions(string idEvent,string idOrganizer);
        Task<List<Event>> GetWeeklyEvents<T>(Pagination paginationOptions);
        Task<List<Event>> GetHighlightedEvents<T>(Pagination paginationOptions);
        Task<Event> Edit<T>(string id, Event eventEdit);
        Task<Event> FindByIdVariant<T>(string id);
    }
}