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
        Task<MessageReturn> GetCardEventsHighligth();
        Task<MessageReturn> GetCardEventsWeekly();
        Task<MessageReturn> GetCardEvents(FilterOptions filters, Pagination paginationOptions);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> GetEventsAsync(FilterOptions filters, Pagination paginationOptions);
        Task<MessageReturn> GetEventsForGridAsync(FilterOptions filters, Pagination paginationOptions);
        Task<MessageReturn> GetWithUserData();
    }
}