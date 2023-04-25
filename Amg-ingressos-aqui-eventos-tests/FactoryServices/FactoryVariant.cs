using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Positions = false,
                Lot = FactoryLot.ListSimpleLot().ToList()
            };
        }
        internal static IEnumerable<Variant> ListSimpleVariant()
        {
            return new List<Variant>()
            {
                new Variant(){
                    Name = "Pista",
                    Positions = false,
                    Lot = FactoryLot.ListSimpleLot().ToList()
                },
                new Variant(){
                    Name = "Camarote",
                    Positions = false,
                    Lot = FactoryLot.ListSimpleLot().ToList()
                },
                new Variant(){
                    Name = "Area VIP",
                    Positions = false,
                    Lot = FactoryLot.ListSimpleLot().ToList()
                },
            };
        }
        internal static IEnumerable<Variant> ListSimpleVariantWithPosition(){
            return new List<Variant>()
            {
                new Variant(){
                    Name = "Assento Normal",
                    Positions = true,
                    Lot = FactoryLot.ListSimpleLotWithPosition().ToList()
                },
                new Variant(){
                    Name = "Camarote",
                    Positions = true,
                    Lot = FactoryLot.ListSimpleLotWithPosition().ToList()
                },
                new Variant(){
                    Name = "Area VIP",
                    Positions = true,
                    Lot = FactoryLot.ListSimpleLotWithPosition().ToList()
                },
            };
        }
    
    }
}