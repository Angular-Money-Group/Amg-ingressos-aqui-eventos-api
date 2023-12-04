using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IEntranceService
    {
        Task<MessageReturn> EntranceTicket(EntranceDto entranceDTO);
    }
}