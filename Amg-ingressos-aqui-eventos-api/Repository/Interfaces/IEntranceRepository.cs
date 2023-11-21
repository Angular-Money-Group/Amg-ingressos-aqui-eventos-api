using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEntranceRepository
    {
        Task<User> GetUserColabData<T>(string idUser);
        Task<object> saveReadyHistories<T>(object ticket);
        Task<EventQrReads> getEventQrReads<T>(string idEvent, string idUser, DateTime initialDate);
        Task<EventQrReads> saveEventQrReads<T>(object eventQr);
        Task<EventQrReads> UpdateEventQrReads<T>(object eventQr);
    }
}
