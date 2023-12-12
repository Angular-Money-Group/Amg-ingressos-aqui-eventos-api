using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Infra;
using MongoDB.Bson;
using Amg_ingressos_aqui_eventos_api.Consts;


namespace Amg_ingressos_aqui_eventos_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TicketRepository<T> : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;

        public TicketRepository(
            IDbConnection<Ticket> dbConnection
        )
        {
            _ticketCollection = dbConnection.GetConnection("tickets");
        }

        public async Task<List<T1>> GetByUser<T1>(string idUser)
        {
            BsonDocument documentFilter = new BsonDocument { { "IdUser", ObjectId.Parse(idUser) } };
            var ticket = await _ticketCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("lots", "IdLot", "_id", "Lots")
                    .Lookup("variants", "Lots.IdVariant", "_id", "Variants")
                    .Lookup("events", "Variants.IdEvent", "_id", "Events")
                    .As<T1>()
                    .ToListAsync();

            return ticket;
        }

        public async Task<object> SaveAsync<T1>(object ticket)
        {
            var data = ticket as Ticket ??
                throw new SaveException(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Ticket(s)"));

            await _ticketCollection.InsertOneAsync(data);
            return data.Id;
        }

        public async Task<object> DeleteMany<T1>(List<string> listId)
        {
            var filtro = Builders<Ticket>.Filter.In("_id", listId);
            var result = await _ticketCollection.DeleteManyAsync(filtro);

            if (result.DeletedCount >= 1)
                return "Ingressos Deletado";
            else
                throw new DeleteException(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteMany), "Ticket(s)"));
        }

        public async Task<object> DeleteByLot<T1>(string idLot)
        {
            var filtro = Builders<Ticket>.Filter.Eq("IdLot", idLot);
            var result = await _ticketCollection.DeleteManyAsync(filtro);

            if (result.DeletedCount >= 1)
                return "Ingressos Deletado";
            else
                throw new DeleteException(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteByLot), "Ticket(s)"));
        }

        public async Task<List<Ticket>> GetTickets<T1>(Ticket ticket)
        {
            var builder = Builders<Ticket>.Filter;
            var filter = builder.Empty;
            filter &= builder.Eq(x => x.IdUser, null);

            if (!string.IsNullOrWhiteSpace(ticket.Id))
                filter &= builder.Eq(x => x.Id, ticket.Id);

            if (!string.IsNullOrEmpty(ticket.IdLot))
                filter &= builder.Eq(x => x.IdLot, ticket.IdLot);

            if (!string.IsNullOrEmpty(ticket.IdUser))
                filter &= builder.Eq(x => x.IdUser, null);

            var result = await _ticketCollection.FindAsync(filter).Result.ToListAsync()
            ?? throw new GetException("Ticket não encontrado");

            return result;
        }

        public async Task<List<string>> GetTicketsByLot<T1>(string idLot)
        {
            var filter = Builders<Ticket>.Filter.Eq("IdLot", idLot);
            var tickets = await _ticketCollection.Find(filter).ToListAsync();

            var result = tickets.Select(e => e.Id).ToList();

            return result;
        }

        public async Task<Ticket> GetById<T1>(string id)
        {
            var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));
            var tickets = await _ticketCollection.Find(filter).FirstOrDefaultAsync();

            var result = tickets ?? throw new GetException("Ticket não encontrado");

            return result;
        }

        public async Task<object> EditAsync<T1>(string id, Ticket ticket)
        {
            var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Ticket>.Update
                .Set("IdUser", ticket.IdUser)
                .Set("Value", ticket.Value)
                .Set("isSold", ticket.IsSold)
                .Set("Position", ticket.Position)
                .Set("QrCode", ticket.QrCode)
                .Set("Status", ticket.Status)
                .Set("IdColab", ticket.IdColab);

            // Busca os tickets que correspondem ao filtro
            await _ticketCollection.UpdateOneAsync(filter, update);

            return await _ticketCollection.Find(filter).ToListAsync();
        }

        public async Task<List<T1>> GetByIdWithDataUser<T1>(string id)
        {
            BsonDocument documentFilter = new BsonDocument { { "_id", ObjectId.Parse(id) } };

            var ticket = await _ticketCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("user", "idUser", "_id", "User")
                    .As<T1>()
                    .ToListAsync();
            return ticket;
        }

        public async Task<List<T1>> GetByIdWithDataEvent<T1>(string id)
        {
            BsonDocument documentFilter = new BsonDocument { { "_id", ObjectId.Parse(id) } };
            var tickets = await _ticketCollection.Aggregate()
                    .Match(documentFilter)
                    .Lookup("lots", "IdLot", "_id", "Lots")
                    .Lookup("variants", "Lots.IdVariant", "_id", "Variants")
                    .Lookup("events", "Variants.IdEvent", "_id", "Events")
                    .As<T1>()
                    .ToListAsync();
            return tickets;
        }

        public async Task<object> SaveMany(List<Ticket> lstTicket)
        {
            await _ticketCollection.InsertManyAsync(lstTicket);
            return "Ok";
        }

        public async Task<object> BurnTicketsAsync<T1>(string id, int status)
        {
            //Where do update
            var filter = Builders<Ticket>.Filter.Eq("_id", ObjectId.Parse(id));

            //Set do update
            var update = Builders<Ticket>.Update.Set("Status", status);

            //Executa o comando
            await _ticketCollection.UpdateOneAsync(filter, update);
            return new object();
        }
    }
}