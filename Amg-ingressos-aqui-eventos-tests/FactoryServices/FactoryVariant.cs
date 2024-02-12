using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Enum;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryVariant
    {
        internal static VariantWithLotDto SimpleVariant()
        {
            return new VariantWithLotDto()
            {
                Name = "Pista",
                HasPositions = false,
                Lots = FactoryLot.ListSimpleLot().ToList()
            };
        }

        internal static IEnumerable<VariantWithLotDto> ListSimpleVariant()
        {
            return new List<VariantWithLotDto>()
            {
                new VariantWithLotDto(){
                    Name = "Pista",
                    HasPositions = false,
                    Lots = FactoryLot.ListSimpleLot().ToList(),
                    Status = StatusVariant.Active
                },
                new VariantWithLotDto(){
                    Name = "Camarote",
                    HasPositions = false,
                    Lots = FactoryLot.ListSimpleLot().ToList(),
                    Status = StatusVariant.Active
                },
                new VariantWithLotDto(){
                    Name = "Area VIP",
                    HasPositions = false,
                    Lots = FactoryLot.ListSimpleLot().ToList(),
                    Status = StatusVariant.Active
                },
            };
        }

        internal static IEnumerable<VariantWithLotDto> ListSimpleVariantWithPosition()
        {
            return new List<VariantWithLotDto>()
            {
                new VariantWithLotDto(){
                    Name = "Assento Normal",
                    HasPositions = true,
                    Lots = FactoryLot.ListSimpleLotWithPosition().ToList(),
                    Positions = FactoryPosition.SimplePosition()
                },
                new VariantWithLotDto(){
                    Name = "Camarote",
                    HasPositions = true,
                    Lots = FactoryLot.ListSimpleLotWithPosition().ToList(),
                    Positions = FactoryPosition.SimplePosition()
                },
                new VariantWithLotDto(){
                    Name = "Area VIP",
                    HasPositions = true,
                    Lots = FactoryLot.ListSimpleLotWithPosition().ToList(),
                    Positions = FactoryPosition.SimplePosition()
                },
            };
        }
    }
}