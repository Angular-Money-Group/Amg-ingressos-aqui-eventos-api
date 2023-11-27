namespace Amg_ingressos_aqui_eventos_api.Repository.Interfaces
{
    public interface IEmailRepository
    {
        public Task<object> SaveAsync(object email);
    }
}