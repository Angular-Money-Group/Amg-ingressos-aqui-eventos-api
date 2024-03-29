using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Consts;
using System.Reflection.Metadata;


namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;

        public TicketRepository(IDbConnection dbConnection)
        {
            _ticketCollection = dbConnection.GetConnection<Ticket>("tickets");
        }

        public async Task<List<T>> GetByUser<T>(string idUser)
        {
            BsonDocument documentFilter = new BsonDocument { { "IdUser", ObjectId.Parse(idUser) } };
            var ticket = await _ticketCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("lots", "IdLot", "_id", "Lots")
                    .Lookup("variants", "Lots.IdVariant", "_id", "Variants")
                    .Lookup("events", "Variants.IdEvent", "_id", "Events")
                    .As<T>()
                    .ToListAsync();

            return ticket;
        }

        public async Task<Ticket> Save(Ticket ticket)
        {
            var data = ticket;
            await _ticketCollection.InsertOneAsync(data);
            return ticket;
        }

        public async Task<bool> DeleteMany(List<string> listId)
        {
            var filtro = Builders<Ticket>.Filter.In("_id", listId);
            var result = await _ticketCollection.DeleteManyAsync(filtro);

            if (result.DeletedCount <= 0)
                throw new DeleteException(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteMany), "Ticket(s)"));
            return true;

        }

        public async Task<bool> DeleteByLot(string idLot)
        {
            var filtro = Builders<Ticket>.Filter.Eq("IdLot", idLot);
            var result = await _ticketCollection.DeleteManyAsync(filtro);

            if (result.DeletedCount <= 0)
                throw new DeleteException(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteByLot), "Ticket(s)"));
            return true;
        }

        public async Task<List<T>> GetTickets<T>(Ticket ticket)
        {
            var builder = Builders<Ticket>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrWhiteSpace(ticket.Id))
                filter &= builder.Eq("_id", ObjectId.Parse(ticket.Id));

            if (!string.IsNullOrEmpty(ticket.IdLot))
                filter &= builder.Eq(x => x.IdLot, ticket.IdLot);

            if (!string.IsNullOrEmpty(ticket.IdUser))
                filter &= builder.Eq(x => x.IdUser, ticket.IdUser);

            var result = await _ticketCollection.Find(filter)
            .As<T>()
            .ToListAsync()
            ?? throw new GetException("Ticket não encontrado");

            return result;
        }

        public async Task<List<string>> GetTicketsByLot(string idLot)
        {
            var filter = Builders<Ticket>.Filter.Eq("IdLot", idLot);
            var tickets = await _ticketCollection.Find(filter).ToListAsync();

            var result = tickets.Select(e => e.Id).ToList();

            return result;
        }

        public async Task<Ticket> GetById(string id)
        {
            var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));
            var tickets = await _ticketCollection.Find(filter).FirstOrDefaultAsync();

            var result = tickets ?? throw new GetException("Ticket não encontrado");

            return result;
        }

        public async Task<List<T>> GetByIdWithDataUser<T>(string id)
        {
            BsonDocument documentFilter = new BsonDocument { { "_id", ObjectId.Parse(id) } };

            var ticket = await _ticketCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("user", "IdUser", "_id", "Users")
                    .As<T>()
                    .ToListAsync();
            return ticket;
        }

        public async Task<List<T>> GetByIdWithDataEvent<T>(string id)
        {
            BsonDocument documentFilter = new BsonDocument { { "_id", ObjectId.Parse(id) } };
            var tickets = await _ticketCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("lots", "IdLot", "_id", "Lots")
                    .Lookup("variants", "Lots.IdVariant", "_id", "Variants")
                    .Lookup("events", "Variants.IdEvent", "_id", "Events")
                    .As<T>()
                    .ToListAsync();
            return tickets;
        }

        public async Task<bool> SaveMany(List<Ticket> lstTicket)
        {
            await _ticketCollection.InsertManyAsync(lstTicket);
            return true;
        }

        public async Task<bool> BurnTicketsAsync(string id, int status)
        {
            //Where do update
            var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));

            //Set do update
            var update = Builders<Ticket>.Update.Set("Status", status);

            //Executa o comando
            await _ticketCollection.UpdateOneAsync(filter, update);
            return true;
        }

        public async Task<List<Ticket>> GetAll()
        {
            var pResult = await _ticketCollection.Find(_ => true)
                .As<Ticket>()
                .ToListAsync();

            return pResult;
        }

        public Task<List<Ticket>> GetByFilter(Dictionary<string, string> filters)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Edit(string id, Ticket model)
        {
            var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Ticket>.Update
                .Set("IdUser", model.IdUser)
                .Set("Value", model.Value)
                .Set("isSold", model.IsSold)
                .Set("Position", model.Position)
                .Set("QrCode", model.QrCode)
                .Set("Status", model.Status)
                .Set("IdColab", model.IdColab)
                .Set("IdLot", model.IdLot);

            // Busca os tickets que correspondem ao filtro
            await _ticketCollection.UpdateOneAsync(filter, update);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Ticket>.Filter.Eq(l => l.Id, id.ToString());
            var deleteResult = await _ticketCollection.DeleteOneAsync(filter);
            if (deleteResult.DeletedCount == 1)
                return true;
            else
                throw new DeleteException("algo deu errado ao deletar");
        }


        public async Task<int> GetCountTicketsNoUser(string idLot)
        {
            //Filta os tickets do lote e que ainda não foram vendidos
            var filter = Builders<Ticket>.Filter.And(
                Builders<Ticket>.Filter.Eq(x => x.IdLot, idLot),
                Builders<Ticket>.Filter.Eq(x => x.IsSold, false)
                );

            var result = await _ticketCollection.Find(filter).CountDocumentsAsync();
            return (int)result;
        }
    }
}