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

        public List<Variant> ListDtoToListModel(List<VariantWithLotDto> listVariant)
        {
            return listVariant.Select(v => DtoToModel(v)).ToList();
        }
        public Variant DtoToModel(VariantWithLotDto variant)
        {
            return new Variant()
            {
                Description = variant.Description,
                HasPositions = variant.HasPositions,
                Id = variant.Id,
                IdEvent = variant.IdEvent,
                LocaleImage = variant.LocaleImage,
                Name = variant.Name,
                Positions = variant.Positions,
                QuantityCourtesy = variant.QuantityCourtesy,
                ReqDocs = variant.ReqDocs,
                SellTicketsBeforeStartAnother = variant.SellTicketsBeforeStartAnother,
                SellTicketsInAnotherBatch = variant.SellTicketsInAnotherBatch,
                Status = variant.Status
            };
        }
    }
}