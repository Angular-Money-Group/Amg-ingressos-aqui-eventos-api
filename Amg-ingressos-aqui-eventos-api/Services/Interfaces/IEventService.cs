using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IEventService
    {
        Task<MessageReturn> SaveAsync(Event eventSave);
        Task<MessageReturn> FindByIdAsync(string id);
        Task<MessageReturn> FindEventByDescriptionAsync(string id);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> GetAllEventsAsync();
        Task<MessageReturn> GetWeeklyEventsAsync(Pagination paginationOptions);
        
        Task<MessageReturn> GetHighlightedEventsAsync(Pagination paginationOptions);
    }
}