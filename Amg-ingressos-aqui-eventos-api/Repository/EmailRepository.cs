using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Infra;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class EmailRepository : IEmailRepository
    {
        private readonly IMongoCollection<Email> _emailCollection;

        public EmailRepository(IDbConnection<Email> dbconnectionIten)
        {
            _emailCollection = dbconnectionIten.GetConnection("templateemails");
        }

        public async Task<object> SaveAsync(object email)
        {
            await _emailCollection.InsertOneAsync((Email)email);
            return (Email)email;
        }
    }
}