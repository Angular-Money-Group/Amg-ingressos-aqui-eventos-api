using Amg_ingressos_aqui_eventos_api.Dto;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class EventComplet : Event
    {
        public EventComplet()
        {
            Variants = new List<Variant>();
            Lots = new List<Lot>();
        }
        public List<Variant> Variants { get; set; }
        public List<Lot> Lots { get; set; }

        public EventCompletDto ModelToDto(EventComplet eventData)
        {
            return new EventCompletDto()
            {
                Address = eventData.Address,
                Courtesy = eventData.Courtesy,
                Description = eventData.Description,
                EndDate = eventData.EndDate,
                Highlighted = eventData.Highlighted,
                Id = eventData.Id,
                IdMeansReceipt = eventData.IdMeansReceipt,
                IdOrganizer = eventData.IdMeansReceipt,
                Image = eventData.Image,
                Local = eventData.Local,
                Name = eventData.Name,
                StartDate = eventData.StartDate,
                Status = eventData.Status,
                Type = eventData.Type,
                Variants = eventData.Variants.Select(v => new VariantWithLotDto()
                {
                    Description = v.Description,
                    HasPositions = v.HasPositions,
                    Id = v.Id,
                    IdEvent = v.IdEvent,
                    LocaleImage = v.LocaleImage,
                    Name = v.Name,
                    Positions = v.Positions,
                    QuantityCourtesy = v.QuantityCourtesy,
                    ReqDocs = v.ReqDocs,
                    SellTicketsBeforeStartAnother = v.SellTicketsBeforeStartAnother,
                    SellTicketsInAnotherBatch = v.SellTicketsInAnotherBatch,
                    Status = v.Status,
                    Lots = eventData.Lots.Where(i => i.IdVariant == v.Id).Select(l => new LotWitTicketDto()
                    {
                        EndDateSales = l.EndDateSales,
                        Id = l.Id,
                        Identificate = l.Identificate,
                        IdVariant = l.IdVariant,
                        ReqDocs = l.ReqDocs,
                        StartDateSales = l.StartDateSales,
                        Status = l.Status,
                        TotalTickets = l.TotalTickets,
                        ValueTotal = l.ValueTotal
                    }).ToList()
                }).ToList()

            };
        }
    }
}