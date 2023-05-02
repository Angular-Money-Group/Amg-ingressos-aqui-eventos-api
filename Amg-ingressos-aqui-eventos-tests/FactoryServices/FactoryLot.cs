using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Enum;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryLot
    {
        internal static Lot SimpleLot()
        {
            return new Lot()
            {
                Identificador = 1,
                StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00),
                EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00),
                TotalTickets = 100,
                ValueTotal = 10000,
                Status = StatusLot.Open
            };
        }
        internal static IEnumerable<Lot> ListSimpleLot()
        {
            return new List<Lot>(){
                new Lot(){
                    Identificador = 1,
                    StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                Status = StatusLot.Open
                },
                new Lot(){
                    Identificador = 2,
                    StartDateSales = new DateTime(2023, 07, 16, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 31, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                Status = StatusLot.Open
                },
                new Lot(){
                    Identificador = 3,
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                Status = StatusLot.Open
                }
            };
        }
        internal static IEnumerable<Lot> ListSimpleLotWithPosition()
        {
            return new List<Lot>(){
                new Lot(){
                    Identificador = 1,
                    StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = StatusLot.Open
        },
                new Lot(){
                    Identificador = 2,
                    StartDateSales = new DateTime(2023, 07, 16, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 31, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = StatusLot.Open
                },
                new Lot(){
                    Identificador = 3,
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = StatusLot.Open
                },
                new Lot(){
                    Identificador = 4,
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = StatusLot.Open
                }
            };
        }
    }
}