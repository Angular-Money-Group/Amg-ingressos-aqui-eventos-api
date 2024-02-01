using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ILotRepository : ICrudRepository<Lot>
    {
        Task<List<Lot>> SaveMany(List<Lot> listLot);
        Task<bool> DeleteByVariant(string idVariant);
        Task<T> GetLotByIdVariant<T>(string idVariant);
        Task<bool> DeleteMany<T>(List<string> listLot);
        Task<List<T>> GetLotByEndDateSales<T>(DateTime dateManagerLots);
        Task<bool> ChangeStatusLot(string id, int statusLot);
        Task<bool> EditCombine(string id, Dictionary<string, string> lotObj);
    }
}