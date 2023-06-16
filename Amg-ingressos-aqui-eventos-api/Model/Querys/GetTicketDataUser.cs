using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetTicketDataUser
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string IdLot { get; set; }
        public object Position { get; set; }
        public decimal Value { get; set; }
        public string IdUser { get; set; }
        public bool isSold { get; set; }
        public string QrCode { get; set; }
        public List<User> User { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EmailConfirmationCode
    {
        [JsonProperty("code")]
        public string code { get; set; }
        
        [JsonProperty("expirationDate")]
        public DateTime expirationDate { get; set; }
    }

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