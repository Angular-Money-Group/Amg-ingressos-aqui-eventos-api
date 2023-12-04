using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket
{
    public class GetEventWithTickets
    {
        public GetEventWithTickets()
        {
            _id = string.Empty;
            Name = string.Empty;
            Local = string.Empty;
            Type = string.Empty;
            Image = string.Empty;
            Description = string.Empty;
            Address = new Address();
            Courtesy = new Courtesy();
            IdMeansReceipt = string.Empty;
            IdOrganizer = string.Empty;
            Variant = new List<Variant>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public string Local { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public Address Address { get; set; }
        public Courtesy Courtesy { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdMeansReceipt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOrganizer { get; set; }
        public bool Highlighted { get; set; }
        public List<Variant> Variant { get; set; }
    }
    public class Address
    {
        public Address()
        {
            Cep = string.Empty;
            AddressDescription = string.Empty;
            Number = string.Empty;
            Neighborhood = string.Empty;
            Complement = string.Empty;
            ReferencePoint = string.Empty;
            City = string.Empty;
            State = string.Empty;
        }
        public string Cep { get; set; }
        public string AddressDescription { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string Complement { get; set; }
        public string ReferencePoint { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class Courtesy
    {
        public Courtesy()
        {
            RemainingCourtesy = new List<RemainingCourtesy>();
            CourtesyHistory = new List<CourtesyHistory>();
        }
        public List<RemainingCourtesy> RemainingCourtesy { get; set; }
        public List<CourtesyHistory> CourtesyHistory { get; set; }
    }

    public class Lot
    {
        public Lot()
        {
            _id = string.Empty;
            IdVariant = string.Empty;
            IdVariant= string.Empty;
            ticket = new List<Model.Ticket>();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int Identificate { get; set; }
        public int TotalTickets { get; set; }
        public decimal ValueTotal { get; set; }
        public DateTime StartDateSales { get; set; }
        public DateTime EndDateSales { get; set; }
        public int Status { get; set; }
        public bool ReqDocs { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdVariant { get; set; }
        public List<Model.Ticket> ticket { get; set; }
    }

    public class RemainingCourtesy
    {
        public RemainingCourtesy(){
            VariantName = string.Empty;
            VariantId = string.Empty;
        }
        public int Quantity { get; set; }
        public string VariantName { get; set; }
        public string VariantId { get; set; }
    }
    public class CourtesyHistory
    {
        public CourtesyHistory(){
            Email= string.Empty;
            IdStatusEmail= string.Empty;
            Variant= string.Empty;
        }
        public string Email { get; set; }
        public string IdStatusEmail { get; set; }
        public DateTime Date { get; set; }
        public string Variant { get; set; }
        public int Quantity { get; set; }
    }

    public class Ticket
    {
        public Ticket()
        {
            _id = string.Empty;
            IdLot = string.Empty;
            IdUser = string.Empty;
            QrCode = string.Empty;
            Position = string.Empty;
            IdColab = string.Empty;
            Status = string.Empty;
        }
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdLot { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get; set; }
        public object Position { get; set; }
        public decimal Value { get; set; }
        public bool isSold { get; set; }
        public object Status { get; set; }
        public object IdColab { get; set; }
        public bool ReqDocs { get; set; }
        public object QrCode { get; set; }
    }

    public class Variant
    {
        public Variant()
        {
            _id = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            IdEvent = string.Empty;
            Lot = new List<Lot>();
            Positions = new Positions();
            LocaleImage = string.Empty;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasPositions { get; set; }
        public int Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }
        public int QuantityCourtesy { get; set; }
        public bool ReqDocs { get; set; }
        public object Positions { get; set; }
        public List<Lot> Lot { get; set; }
        public string? LocaleImage { get; set; }
    }
}