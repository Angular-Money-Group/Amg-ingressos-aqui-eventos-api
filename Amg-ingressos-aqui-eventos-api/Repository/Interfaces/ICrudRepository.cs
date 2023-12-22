namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface ICrudRepository<T>
    {
        Task<T> GetById(string id);
        Task<List<T>> GetAll();
        Task<List<T>> GetByFilter(Dictionary<string,string> filters);
        Task<T> Save(T model);
        Task<bool> Edit(string id, T model);
        Task<bool> Delete(string id);
    }
}