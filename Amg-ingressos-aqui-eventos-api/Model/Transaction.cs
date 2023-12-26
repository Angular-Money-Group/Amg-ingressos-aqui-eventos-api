using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Transaction
    {
        public Transaction()
        {
            Id = string.Empty;
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
        [JsonPropertyName("id")]
        [JsonProperty("_id")]
        public string Id { get; set; }
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
}