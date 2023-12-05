using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class VariantWithLotDto : Variant
    {
        public VariantWithLotDto()
        {
            Lots = new List<LotWithTicketDto>();
        }
        public List<LotWithTicketDto> Lots { get; set; }
    }
}