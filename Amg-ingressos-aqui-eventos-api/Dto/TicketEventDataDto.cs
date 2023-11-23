namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class TicketEventDataDto
    {
        public TicketEventDataDto()
        {
            Lot = new Model.Lot();
            Variant = new Model.Variant();
            Event = new Model.Event();
            Id = string.Empty;
            IdLot = string.Empty;
            IdUser = string.Empty;
            Position = string.Empty;
            QrCode = string.Empty;
        }

        public Model.Lot Lot { get; set; }
        public Model.Variant Variant { get; set; }
        public Model.Event Event { get; set; }
        public string Id { get; set; }
        public string IdLot { get; set; }
        public string IdUser { get; set; }
        public object Position { get; set; }
        public decimal Value { get; set; }
        public bool IsSold { get; set; }
        public bool ReqDocs { get; set; }
        public string QrCode { get; set; }
    }
}