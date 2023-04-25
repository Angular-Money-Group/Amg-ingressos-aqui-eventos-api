using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryLot
    {
        internal static Lot SimpleLot()
        {
            return new Lot()
            {
                Description = "Lote1",
                StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00),
                EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00),
                TotalTickets = 100,
                ValueTotal = 10000
            };
        }
        internal static IEnumerable<Lot> ListSimpleLot()
        {
            return new List<Lot>(){
                new Lot(){
                    Description = "Lote1",
                    StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000
                },
                new Lot(){
                    Description = "Lote2",
                    StartDateSales = new DateTime(2023, 07, 16, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 31, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000
                },
                new Lot(){
                    Description = "Lote3",
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000
                }
            };
        }
        internal static IEnumerable<Lot> ListSimpleLotWithPosition()
        {
            return new List<Lot>(){
                new Lot(){
                    Description = "Lote1",
                    StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Positions = FactoryEvent.SimplePosition()
        },
                new Lot(){
                    Description = "Lote2",
                    StartDateSales = new DateTime(2023, 07, 16, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 31, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Positions = FactoryEvent.SimplePositionWithSoldPositions()
                },
                new Lot(){
                    Description = "Lote3",
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Positions = FactoryEvent.SimplePositionWithReservedPositions()
                },
                new Lot(){
                    Description = "Lote3",
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Positions = FactoryEvent.SimplePositionWithReservedPositionsAndSoldPositions()
                }

            };

        }

    }
}