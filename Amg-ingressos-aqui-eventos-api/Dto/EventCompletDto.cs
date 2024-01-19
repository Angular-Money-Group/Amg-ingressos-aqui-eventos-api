using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EventCompletDto : Event
    {
        public EventCompletDto()
        {
            Variants = new List<VariantWithLotDto>();
            NameOrganizer = string.Empty;
        }
        public List<VariantWithLotDto> Variants { get; set; }
        public string NameOrganizer { get; set; }

        public Event DtoToModel(EventCompletDto eventData)
        {
            return new Event()
            {
                Address = eventData.Address,
                Courtesy = eventData.Courtesy,
                Description = eventData.Description,
                EndDate = eventData.EndDate,
                Highlighted = eventData.Highlighted,
                Id = eventData.Id,
                IdMeansReceipt = eventData.IdMeansReceipt,
                IdOrganizer = eventData.IdOrganizer,
                Image = eventData.Image,
                Local = eventData.Local,
                Name = eventData.Name,
                StartDate = eventData.StartDate,
                Status = eventData.Status,
                Type = eventData.Type
            };
        }
    }
}