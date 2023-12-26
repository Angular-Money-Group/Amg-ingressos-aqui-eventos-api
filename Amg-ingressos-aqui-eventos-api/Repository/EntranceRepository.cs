using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class EntranceRepository : IEntranceRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<ReadHistory> _readHistoryCollection;
        private readonly IMongoCollection<EventQrReads> _eventQrCollection;

        public EntranceRepository(
            IDbConnection<User> dbUser,
            IDbConnection<ReadHistory> dbReadyHistory,
            IDbConnection<EventQrReads> dbEventQrReads)
        {
            _userCollection = dbUser.GetConnection("user");
            _readHistoryCollection = dbReadyHistory.GetConnection("readhistories");
            _eventQrCollection = dbEventQrReads.GetConnection("eventqrreads");
        }

        public async Task<User> GetUserColabData(string idUser)
        {
            if (idUser == null || string.IsNullOrEmpty(idUser.ToString()))
                throw new GetException("id é obrigatório");

            var filter = Builders<User>.Filter.Eq("Id", idUser);
            User user = await _userCollection.Find(filter).FirstOrDefaultAsync() ?? new User();
            return user;
        }

        public async Task<ReadHistory> SaveReadyHistories(object ticket)
        {

            var data = ticket as ReadHistory ?? throw new RuleException("Ticket é obrigatorio");
            await _readHistoryCollection.InsertOneAsync(data);
            return data;
        }

        public async Task<EventQrReads> GetEventQrReads(string idEvent, string idUser, DateTime initialDate)
        {
            EventQrReads eventQrs = new EventQrReads();
            //Monta lista de campos para find na collection
            var filter = Builders<EventQrReads>.Filter.And(
                Builders<EventQrReads>.Filter.Where(eventQr => eventQr.IdColab.Contains(idUser)),
                Builders<EventQrReads>.Filter.Where(eventQr => eventQr.IdEvent.Contains(idEvent)),
                Builders<EventQrReads>.Filter.Gte(eventQr => eventQr.InitialDate, initialDate.Date),
                Builders<EventQrReads>.Filter.Lt(eventQr => eventQr.InitialDate, initialDate.Date.AddDays(1).AddSeconds(-1))
                );

            eventQrs = await _eventQrCollection.Find(filter).FirstOrDefaultAsync();

            return eventQrs;
        }

        public async Task<EventQrReads> SaveEventQrReads(object eventQr)
        {

            var data = eventQr as EventQrReads ?? throw new RuleException("EventQrRead não pode ser null");
            await _eventQrCollection.InsertOneAsync(data);

            return data;
        }

        public async Task<EventQrReads> EditEventQrReads(object eventQr)
        {
            //tipa o objeto
            var eventLocal = eventQr as EventQrReads ?? throw new EditException("EventQrRead é obrigatório");

            //Monta lista de campos, que serão atualizado - Set do update
            var updateDefination = new List<UpdateDefinition<EventQrReads>>
                {
                    Builders<EventQrReads>.Update.Set(eventQr => eventQr.IdColab, eventLocal.IdColab),
                    Builders<EventQrReads>.Update.Set(eventQr => eventQr.IdEvent, eventLocal.IdEvent),
                    Builders<EventQrReads>.Update.Set(eventQr => eventQr.TotalReads, eventLocal.TotalReads),
                    Builders<EventQrReads>.Update.Set(eventQr => eventQr.TotalSuccess, eventLocal.TotalSuccess),
                    Builders<EventQrReads>.Update.Set(eventQr => eventQr.TotalFail, eventLocal.TotalFail),
                    Builders<EventQrReads>.Update.Set(eventQr => eventQr.LastRead, eventLocal.LastRead)
                };

            if (eventLocal.Status != 0)
            {
                updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.Status, eventLocal.Status));
            }

            //Where do update
            var filter = Builders<EventQrReads>.Filter.Eq("_id", ObjectId.Parse(eventLocal.Id));

            //Prepara o objeto para atualização
            var combinedUpdate = Builders<EventQrReads>.Update.Combine(updateDefination);

            //Realiza o update
            await _eventQrCollection.UpdateOneAsync(filter, combinedUpdate);
            return eventQr as EventQrReads ?? new EventQrReads();
        }
    }
}