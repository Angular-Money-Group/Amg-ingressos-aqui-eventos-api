using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Services.Interfaces
{
    public interface IEmailService
    {
        Task<MessageReturn> SaveAsync(Email email);
        Task<MessageReturn> Send(string idEmail, StatusTicketsRow ticketsRow, int index, string rowId);
        string GenerateBody();
    }
}