using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Sockets;

namespace Amg_ingressos_aqui_eventos_api.Repository
{
    public class EntranceRepository<T> : IEntranceRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<ReadHistory> _readHistoryCollection;
        private readonly IMongoCollection<EventQrReads> _eventQrCollection;
        private readonly MongoClient _mongoClient;

        public EntranceRepository(
            IDbConnection<User> dbUser,
            IDbConnection<ReadHistory> dbReadyHistory,
            IDbConnection<EventQrReads> dbEventQrReads)
        {
            _mongoClient = dbUser.GetClient();
            _userCollection = dbUser.GetConnection("user");
            _readHistoryCollection = dbReadyHistory.GetConnection("readhistories");
            _eventQrCollection = dbEventQrReads.GetConnection("eventqrreads");
        }

        public async Task<User> GetUserColabData<T>(string idUser)
        {
            User user = new User();

            try
			{
                var filter = Builders<User>.Filter.Eq("Id", idUser);
                user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
			{
				throw;
			}

            return user;
        }

        public async Task<object> saveReadyHistories<T>(object ticket)
        {
            try
            {
                await _readHistoryCollection.InsertOneAsync(ticket as ReadHistory);
                return ((ReadHistory)ticket).Id;
            }
            catch (SaveTicketException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EventQrReads> getEventQrReads<T>(string idEvent, string idUser, DateTime initialDate)
        {
            EventQrReads eventQrs = new EventQrReads();

            try
            {
                //Monta lista de campos para find na collection
                var filter = Builders<EventQrReads>.Filter.And(
                    Builders<EventQrReads>.Filter.Where(eventQr => eventQr.IdColab.Contains(idUser)),
                    Builders<EventQrReads>.Filter.Where(eventQr => eventQr.IdEvent.Contains(idEvent)),
                    Builders<EventQrReads>.Filter.Gte(eventQr => eventQr.InitialDate, initialDate.Date),
                    Builders<EventQrReads>.Filter.Lt(eventQr => eventQr.InitialDate, initialDate.Date.AddDays(1).AddSeconds(-1))
                    );

                eventQrs = await _eventQrCollection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            return eventQrs;
        }

        public async Task<EventQrReads> saveEventQrReads<T>(object eventQr)
        {
            try
            {
                //Qual é o retorno ?
                await _eventQrCollection.InsertOneAsync(eventQr as EventQrReads);
                //return ((EventQrReads)eventQr).Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            return eventQr as EventQrReads;
        }

        public async Task<EventQrReads> UpdateEventQrReads<T>(object eventQr)
        {
            try
            {
                //tipa o objeto
                var eventLocal = eventQr as EventQrReads;

                //Monta lista de campos, que serão atualizado - Set do update
                var updateDefination = new List<UpdateDefinition<EventQrReads>>();
                updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.IdColab, eventLocal.IdColab));
                updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.IdEvent, eventLocal.IdEvent));
                updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.TotalReads, eventLocal.TotalReads));
                updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.TotalSuccess, eventLocal.TotalSuccess));
                updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.TotalFail, eventLocal.TotalFail));
                updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.LastRead, eventLocal.LastRead));
                //updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.loginHistory, eventLocal.loginHistory));
                //updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.readHistory, eventLocal.readHistory));

                if (eventLocal.Status != null)
                {
                    updateDefination.Add(Builders<EventQrReads>.Update.Set(eventQr => eventQr.Status, eventLocal.Status));
                }

                //Where do update
                var filter = Builders<EventQrReads>.Filter.Eq("_id", ObjectId.Parse(eventLocal.Id));

                //Prepara o objeto para atualização
                var combinedUpdate = Builders<EventQrReads>.Update.Combine(updateDefination);

                //Realiza o update
                UpdateResult updateResult = await _eventQrCollection.UpdateOneAsync(filter, combinedUpdate, new UpdateOptions() { });
            }
            catch (Exception ex)
            {
                throw;
            }

            return eventQr as EventQrReads;
        }
    }
}
