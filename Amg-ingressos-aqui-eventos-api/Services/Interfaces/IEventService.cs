using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IEventService
    {
        Task<MessageReturn> SaveAsync(Event eventSave);
        Task<MessageReturn> FindByIdAsync(string id);
        Task<MessageReturn> HighlightEventAsync(string id);
        Task<MessageReturn> EditEventsAsync(string id, Event eventEdit);
        Task<MessageReturn> GetAllEventsWithTickets(string idEvent);
        Task<MessageReturn> FindEventByNameAsync(string name);
        Task<MessageReturn> FindByOrganizerAsync(string idOrganizer, Pagination paginationOptions, FilterOptions? filter);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> GetEventsAsync(bool highlights, bool weekly,   Pagination paginationOptions);
        Task<MessageReturn> GetAllEventsAsync();
    }
}