using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class TicketUserDataDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? IdLot { get; set; }
        public string? IdUser { get; set; }
        public string? Position { get; set; }
        public decimal Value { get; set; }
        public bool isSold { get; set; }
        public string QrCode { get; set; }
        public UserDto User { get; set; }
    }
}