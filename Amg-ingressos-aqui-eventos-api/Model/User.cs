using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class User
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string name { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phoneNumber { get; set; }
        public bool isPhoneVerified { get; set; }
        public bool isEmailVerified { get; set; }
        public bool isActive { get; set; }
        public int __v { get; set; }
        public List<object> tickets { get; set; }
        public object emailConfirmationCode { get; set; }
        public List<string> paymentMethods { get; set; }
    }
}