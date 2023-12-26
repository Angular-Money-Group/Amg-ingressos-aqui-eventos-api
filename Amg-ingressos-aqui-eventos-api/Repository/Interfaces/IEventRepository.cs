using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEventRepository : ICrudRepository<Event>
    {

        Task<T> GetById<T>(string id);
        Task<List<T>> GetByFilterComplet<T>(Pagination paginationOptions, Event eventModel);
        Task<List<T>> GetFilterWithTickets<T>(string idEvent, string idOrganizer);
        Task<List<T>> GetFilterWithTransactions<T>(string idEvent, string idOrganizer);
        Task<List<T>> GetByFilter<T>(Dictionary<string, object> filters, Pagination? paginationOptions);
        Task<bool> SetHighlightEvent(string id);
    }
}