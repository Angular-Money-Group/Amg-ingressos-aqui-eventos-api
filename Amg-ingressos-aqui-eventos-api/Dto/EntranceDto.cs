using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EntranceDto
    {
        public string IdUser { get; set; }
        public string IdEvent { get; set; }
        public string IdTicket { get; set; }
    }

    public class ResponseEntranceDTO
    {
        public Boolean reqDocs { get; set; }
        public string userName { get; set; }
        public string userDoc { get; set; }
        public string userEmail { get; set; }
        public string variantName { get; set; }
    }
}
