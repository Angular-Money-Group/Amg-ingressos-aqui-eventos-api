using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EventCompletDto : Event
    {
        public EventCompletDto()
        {
            Variants = new List<VariantWithLotDto>();
            Transactions = new List<Transaction>();
            NameOrganizer = string.Empty;
        }
        public List<VariantWithLotDto> Variants { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string NameOrganizer { get; set; }
    }
}