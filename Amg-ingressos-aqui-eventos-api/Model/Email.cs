using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Email
    {
        public Email()
        {
            Id = string.Empty; 
            Sender = string.Empty;
            To = string.Empty;
            Subject = string.Empty;
            Attachments = string.Empty;
            Body = string.Empty;
            Status = string.Empty;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;
        public string Sender { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Attachments { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime Dataenvio { get; set; }
    }
}