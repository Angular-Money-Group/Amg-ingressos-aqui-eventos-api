using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface ILotService
    {
        Task<MessageReturn> SaveAsync(LotWithTicketDto lot);
        Task<MessageReturn> SaveManyAsync(List<LotWithTicketDto> listLot);
        Task<MessageReturn> EditAsync(string id, Lot lot);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> DeleteByVariantAsync(string idVariant);
        Task<MessageReturn> DeleteManyAsync(List<string> listLot);
        Task<MessageReturn> GetByIdVariant(string idVariant);
    }
}