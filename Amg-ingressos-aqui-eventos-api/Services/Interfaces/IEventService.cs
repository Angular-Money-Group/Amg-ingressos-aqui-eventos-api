using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IEventService
    {
        Task<MessageReturn> SaveAsync(EventCompletDto eventObject);
        Task<MessageReturn> GetByIdAsync(string id);
        Task<MessageReturn> SetHighlightEventAsync(string id);
        Task<MessageReturn> EditEventsAsync(string id, EventEditDto eventDto);
        Task<MessageReturn> GetAllEventsWithTickets(string idEvent);
        Task<MessageReturn> GetByNameAsync(string name);
        Task<MessageReturn> GetByOrganizerAsync(string idOrganizer, Pagination paginationOptions, FilterOptions? filter);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> GetEventsAsync(bool highlights, bool weekly,   Pagination paginationOptions);
        Task<MessageReturn> GetWithUserData();
    }
}