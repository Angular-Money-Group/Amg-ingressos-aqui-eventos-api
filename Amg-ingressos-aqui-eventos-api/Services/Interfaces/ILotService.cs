using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface ILotService
    {
        Task<MessageReturn> SaveAsync(Lot lot);
        Task<MessageReturn> VerifyLotsAvaibleAsync(string idVariant);
    }
}