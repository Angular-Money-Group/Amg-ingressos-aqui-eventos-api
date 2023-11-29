using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Enum;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryVariant
    {
        internal static Variant SimpleVariant()
        {
            return new Variant()
            {
                Name = "Pista",
                HasPositions = false,
                Lot = FactoryLot.ListSimpleLot().ToList()
            };
        }
        internal static IEnumerable<Variant> ListSimpleVariant()
        {
            return new List<Variant>()
            {
                new Variant(){
                    Name = "Pista",
                    HasPositions = false,
                    Lot = FactoryLot.ListSimpleLot().ToList(),
                    Status = EnumStatusVariant.Active
                },
                new Variant(){
                    Name = "Camarote",
                    HasPositions = false,
                    Lot = FactoryLot.ListSimpleLot().ToList(),
                    Status = EnumStatusVariant.Active
                },
                new Variant(){
                    Name = "Area VIP",
                    HasPositions = false,
                    Lot = FactoryLot.ListSimpleLot().ToList(),
                    Status = EnumStatusVariant.Active
                },
            };
        }
        internal static IEnumerable<Variant> ListSimpleVariantWithPosition(){
            return new List<Variant>()
            {
                new Variant(){
                    Name = "Assento Normal",
                    HasPositions = true,
                    Lot = FactoryLot.ListSimpleLotWithPosition().ToList(),
                    Positions = FactoryPosition.SimplePosition()
                },
                new Variant(){
                    Name = "Camarote",
                    HasPositions = true,
                    Lot = FactoryLot.ListSimpleLotWithPosition().ToList(),
                    Positions = FactoryPosition.SimplePosition()
                },
                new Variant(){
                    Name = "Area VIP",
                    HasPositions = true,
                    Lot = FactoryLot.ListSimpleLotWithPosition().ToList(),
                    Positions = FactoryPosition.SimplePosition()
                },
            };
        }
    
    }
}