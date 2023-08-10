using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Infra;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IMongoCollection<Email> _emailCollection;
        public EmailRepository(IDbConnection<Email> dbconnectionIten)
        {
            _emailCollection = dbconnectionIten.GetConnection("templateemails");
        }
        public async Task<object> SaveAsync(object email)
        {
            try
            {
                await _emailCollection.InsertOneAsync((Email)email);
                return (Email)email;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}