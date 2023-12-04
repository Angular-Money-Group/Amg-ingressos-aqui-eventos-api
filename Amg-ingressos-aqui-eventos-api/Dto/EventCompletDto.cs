using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EventCompletDto : Event
    {
        public EventCompletDto()
        {
            Variants = new List<VariantWithLotDto>();
        }
        public List<VariantWithLotDto> Variants { get; set; }
    }
}