using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class TicketCompletDto : Ticket
    {
        public TicketCompletDto()
        {
            Lot = new Lot();
            Variant = new Variant();
            Event = new Event();
        }

        public Lot Lot { get; set; }
        public Variant Variant { get; set; }
        public Event Event { get; set; }

        internal IEnumerable<TicketCompletDto> ModelListToDtoList(List<TicketComplet> data)
        {
            return data.Select(t => ModelToDto(t));
        }
        internal TicketCompletDto ModelToDto(TicketComplet data)
        {
            return new TicketCompletDto()
            {
                Event = data.Events.Find(e => e.Id == data?.Variants?.Find(v => v.Id == data?.Lots?.Find(l => l.Id == data?.IdLot)?.IdVariant)?.IdEvent) ?? new Event(),
                Id = data.Id,
                IdColab = data.IdColab,
                IdLot = data.IdLot,
                IdUser = data.IdUser,
                IsSold = data.IsSold,
                Lot = data.Lots.Find(l => l.Id == data.IdLot) ?? new Lot(),
                Position = data.Position,
                QrCode = data.QrCode,
                ReqDocs = data.ReqDocs,
                Status = data.Status,
                TicketCortesia = data.TicketCortesia,
                Value = data.Value,
                Variant = data.Variants.Find(v => v.Id == data?.Lots?.Find(l => l.Id == data.IdLot)?.IdVariant) ?? new Variant()
            };
        }
    }
}