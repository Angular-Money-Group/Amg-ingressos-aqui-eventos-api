using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventTransactions
{
    public class GetEventTransactions : Event
    {
        public List<Transaction> Transaction { get; set; }
    }

    public class Transaction
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPerson { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Tax { get; set; }
        public double TotalValue { get; set; }
        public string Discount { get; set; }
        public int Status { get; set; }
        public Enum.EnumStageTransaction Stage { get; set; }
        public object ReturnUrl { get; set; }
        public object PaymentIdService { get; set; }
        public object Details { get; set; }
        public int TotalTicket { get; set; }
        public DateTime DateRegister { get; set; }
        public List<TransactionIten> TransactionItens { get; set; }
    }

    public class TransactionIten
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTransaction { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTicket { get; set; }
        public bool HalfPrice { get; set; }
        public string TicketPrice { get; set; }
        public string Details { get; set; }
    }

    public class PaymentMethod
    {
        public string IdPaymentMethod { get; set; }
        public Enum.EnumTypePayment TypePayment { get; set; }
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public bool SaveCard { get; set; }
        public string Brand { get; set; }
        public int Installments { get; set; }
    }
}
