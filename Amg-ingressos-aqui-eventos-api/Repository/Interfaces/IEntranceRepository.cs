using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEntranceRepository
    {
        Task<User> GetUserColabData<T>(string idUser);
        Task<object> SaveReadyHistories<T>(object ticket);
        Task<EventQrReads> GetEventQrReads<T>(string idEvent, string idUser, DateTime initialDate);
        Task<EventQrReads> SaveEventQrReads<T>(object eventQr);
        Task<EventQrReads> UpdateEventQrReads<T>(object eventQr);
    }
}