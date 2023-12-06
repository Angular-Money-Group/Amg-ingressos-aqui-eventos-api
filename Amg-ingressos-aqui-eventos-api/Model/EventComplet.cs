using System.Transactions;
using Amg_ingressos_aqui_eventos_api.Dto;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class EventComplet : Event
    {
        public EventComplet()
        {
            Variants = new List<Variant>();
            Lots = new List<Lot>();
            User = new List<User>();
            Tickets = new List<Ticket>();
            Transactions = new List<Transaction>();
        }
        public List<Variant> Variants { get; set; }
        public List<Lot> Lots { get; set; }
        public List<User> User { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Transaction> Transactions { get; set; }

        public List<EventCompletWithTransactionDto> ModelListToDtoList(List<EventComplet> listEventData)
        {
            return listEventData.Select(ModelToDto).ToList();
        }
        public EventCompletWithTransactionDto ModelToDto(EventComplet eventData)
        {
            return new EventCompletWithTransactionDto()
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
                NameOrganizer = eventData?.User?.FirstOrDefault()?.Name ?? string.Empty,
                Transactions = eventData?.Transactions ?? new List<Transaction>(),
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
                    Lots = eventData.Lots.Where(i => i.IdVariant == v.Id).Select(l => new LotWithTicketDto()
                    {
                        EndDateSales = l.EndDateSales,
                        Id = l.Id,
                        Identificate = l.Identificate,
                        IdVariant = l.IdVariant,
                        ReqDocs = l.ReqDocs,
                        StartDateSales = l.StartDateSales,
                        Status = l.Status,
                        TotalTickets = l.TotalTickets,
                        ValueTotal = l.ValueTotal,
                        Tickets = eventData.Tickets.Where(x => x.IdLot == l.Id).Select(t => new Ticket()
                        {
                            Id = t.Id,
                            IdColab = t.IdColab,
                            IdLot = t.IdLot,
                            IdUser = t.IdUser,
                            IsSold = t.IsSold,
                            Position = t.Position,
                            QrCode = t.QrCode,
                            ReqDocs = t.ReqDocs,
                            Status = t.Status,
                            TicketCortesia = t.TicketCortesia,
                            Value = t.Value
                        }).ToList()
                    }).ToList()
                }).ToList()

            };
        }
    }
}