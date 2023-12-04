using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ILotRepository 
    {
        Task<object> Save<T>(object lot);
        Task<object> SaveMany<T>(List<Lot> listLot);
        Task<Lot> Edit<T>(string id, Lot lotObj);
        Task<object> Delete<T>(object id);
        Task<object> DeleteByVariant<T>(object idVariant);
        Task<Lot> GetLotByIdVariant<T>(string idVariant);
        Task<object> DeleteMany<T>(List<string> listLot);
    }
}