using Amg_ingressos_aqui_eventos_api.Enum;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class TicketEventDataDto
    {
        public Lot lot { get; set; }
        public Variant variant { get; set; }
        public Event @event { get; set; }
        public string id { get; set; }
        public string idLot { get; set; }
        public string idUser { get; set; }
        public object position { get; set; }
        public decimal value { get; set; }
        public bool isSold { get; set; }
        public bool reqDocs { get; set; }
        public string qrCode { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public string cep { get; set; }
        public string addressDescription { get; set; }
        public string number { get; set; }
        public string neighborhood { get; set; }
        public string complement { get; set; }
        public string referencePoint { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class Event
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string local { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public StatusEvent status { get; set; }
        public Address address { get; set; }
        public string idMeansReceipt { get; set; }
        public string idOrganizer { get; set; }
        public bool highlighted { get; set; }
    }

    public class Lot
    {
        public string _id { get; set; }
        public int identificate { get; set; }
        public int totalTickets { get; set; }
        public decimal valueTotal { get; set; }
        public DateTime startDateSales { get; set; }
        public DateTime endDateSales { get; set; }
        public StatusLot status { get; set; }
        public bool reqDocs { get; set; }
        public string idVariant { get; set; }
    }

    public class Variant
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool hasPositions { get; set; }
        public StatusVariant status { get; set; }
        public string idEvent { get; set; }
        public bool reqDocs { get; set; }
        public object positions { get; set; }
    }
}