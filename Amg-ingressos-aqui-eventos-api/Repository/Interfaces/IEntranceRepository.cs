using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEntranceRepository
    {
        Task<User> GetUserColabData(string idUser);
        Task<ReadHistory> SaveReadyHistories(object ticket);
        Task<EventQrReads> GetEventQrReads(string idEvent, string idUser, DateTime initialDate);
        Task<EventQrReads> SaveEventQrReads(object eventQr);
        Task<EventQrReads> EditEventQrReads(object eventQr);
    }
}