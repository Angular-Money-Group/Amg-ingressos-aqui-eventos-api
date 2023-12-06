using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Enum;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryLot
    {
        internal static LotWithTicketDto SimpleLot()
        {
            return new LotWithTicketDto()
            {
                Identificate = 1,
                StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00, DateTimeKind.Local),
                EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00, DateTimeKind.Local),
                TotalTickets = 100,
                ValueTotal = 10000,
                Status = EnumStatusLot.Open
            };
        }
        internal static IEnumerable<LotWithTicketDto> ListSimpleLot()
        {
            return new List<LotWithTicketDto>(){
                new LotWithTicketDto(){
                    Identificate = 1,
                    StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00, DateTimeKind.Local),
                    EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00, DateTimeKind.Local),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                Status = EnumStatusLot.Open
                },
                new LotWithTicketDto(){
                    Identificate = 2,
                    StartDateSales = new DateTime(2023, 07, 16, 00, 00, 00, DateTimeKind.Local),
                    EndDateSales = new DateTime(2023, 07, 31, 16, 00, 00, DateTimeKind.Local),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                Status = EnumStatusLot.Open
                },
                new LotWithTicketDto(){
                    Identificate = 3,
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00, DateTimeKind.Local),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00, DateTimeKind.Local),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                Status = EnumStatusLot.Open
                }
            };
        }
        internal static IEnumerable<LotWithTicketDto> ListSimpleLotWithPosition()
        {
            return new List<LotWithTicketDto>(){
                new LotWithTicketDto(){
                    Identificate = 1,
                    StartDateSales = new DateTime(2023, 07, 01, 00, 00, 00, DateTimeKind.Local),
                    EndDateSales = new DateTime(2023, 07, 15, 16, 00, 00, DateTimeKind.Local),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = EnumStatusLot.Open
        },
                new LotWithTicketDto(){
                    Identificate = 2,
                    StartDateSales = new DateTime(2023, 07, 16, 00, 00, 00, DateTimeKind.Local),
                    EndDateSales = new DateTime(2023, 07, 31, 16, 00, 00, DateTimeKind.Local),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = EnumStatusLot.Open
                },
                new LotWithTicketDto(){
                    Identificate = 3,
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00, DateTimeKind.Local),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00, DateTimeKind.Local),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = EnumStatusLot.Open
                },
                new LotWithTicketDto(){
                    Identificate = 4,
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00, DateTimeKind.Local),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00, DateTimeKind.Local),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    Status = EnumStatusLot.Open
                }
            };
        }
    }
}