using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventTransactions
{
    public class GetEventTransactions : Event
    {
        public GetEventTransactions()
        {
            Transaction = new List<Transaction>();
        }
        public List<Transaction> Transaction { get; set; }
    }

    public class Transaction
    {
        public Transaction()
        {
            _id = string.Empty;
            IdPerson = string.Empty;
            IdEvent = string.Empty;
            PaymentMethod = new PaymentMethod();
            Tax = string.Empty;
            Discount = string.Empty;
            ReturnUrl = string.Empty;
            PaymentIdService = string.Empty;
            Details = string.Empty;
            TransactionItens = new List<TransactionIten>();
        }

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
        public TransactionIten(){
            _id = string.Empty;
            IdTransaction = string.Empty;
            IdTicket = string.Empty;
            TicketPrice = string.Empty;
            Details = string.Empty;
        }

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
        public PaymentMethod(){
            IdPaymentMethod = string.Empty;
            CardNumber = string.Empty;
            Holder = string.Empty;
            ExpirationDate = string.Empty;
            SecurityCode = string.Empty;
            Brand = string.Empty;
        }

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