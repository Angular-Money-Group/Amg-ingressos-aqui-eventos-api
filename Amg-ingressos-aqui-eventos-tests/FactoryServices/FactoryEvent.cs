using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryEvent
    {
        internal static Event SimpleEvent()
        {
            return new Event()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a962",
                Name = "Gustavo Lima",
                Local = "Arena Race",
                Type = "Show",
                Image = "http://localhost/image.jpg",
                Description = "Lorem Ipsum is simply dummy text of the printing" +
                "and typesetting industry. Lorem Ipsum has been the industry's" +
                 "standard dummy text ever since the 1500s,",
                Address = new Address()
                {
                    AddressDescription = "Arena Race",
                    Cep = "38400000",
                    Number = "n/a",
                    Neighborhood = "n/a",
                    Complement = "n/a",
                    ReferencePoint = "n/a",
                    City = "Uberlandia",
                    State = "MG",
                },
                StartDate = new DateTime(2023, 10, 30, 16, 00, 00),
                EndDate = new DateTime(2023, 10, 31, 05, 00, 00),
                IdMeansReceipt = "3b241101-e2bb-4255-8caf-4136c566a962",
                IdOrganizer = "3b241101-e2bb-4255-8caf-4136c566a962",
                Variant = ListSimpleVariant().ToList(),
            };
        }
        internal static Event SimpleEventWithPosition()
        {
            return new Event()
            {
                Id = "2b222202-e2bb-4255-8caf-4136c566a962",
                Name = "CB LOL",
                Local = "Parque Sabiázinho",
                Type = "Gamer",
                Image = "http://localhost/image.jpg",
                Description = "Lorem Ipsum is simply dummy text of the printing" +
                "and typesetting industry. Lorem Ipsum has been the industry's" +
                 "standard dummy text ever since the 1500s,",
                Address = new Address()
                {
                    AddressDescription = "Parque Sabiázinho",
                    Cep = "38400000",
                    Number = "N/A",
                    Neighborhood = "teste",
                    Complement = "N/A",
                    ReferencePoint = "n/a",
                    City = "Uberlandia",
                    State = "MG",
                },
                StartDate = new DateTime(2024, 02, 01, 16, 00, 00),
                EndDate = new DateTime(2024, 02, 01, 22, 00, 00),
                IdMeansReceipt = "3b241101-e2bb-4255-8caf-4136c566a962",
                IdOrganizer = "3b241101-e2bb-4255-8caf-4136c566a962",
                Variant = ListSimpleVariantWithPosition().ToList(),
            };
        }

        internal static IEnumerable<Event> ListSimpleEvent()
        {
            List<Event> listEvent = new List<Event>();
            listEvent.Add(SimpleEvent());
            listEvent.Add(SimpleEventWithPosition());

            return listEvent;
        }
        internal static Variant SimpleVariant()
        {
            return new Variant()
            {
                Name = "Pista",
                positions = false,
                Lot = ListSimpleLot().ToList()
            };
        }
        internal static IEnumerable<Variant> ListSimpleVariant()
        {
            return new List<Variant>()
            {
                new Variant(){
                    Name = "Pista",
                    positions = false,
                    Lot = ListSimpleLot().ToList()
                },
                new Variant(){
                    Name = "Camarote",
                    positions = false,
                    Lot = ListSimpleLot().ToList()
                },
                new Variant(){
                    Name = "Area VIP",
                    positions = false,
                    Lot = ListSimpleLot().ToList()
                },
            };
        }
        internal static IEnumerable<Variant> ListSimpleVariantWithPosition()
        {
            return new List<Variant>()
            {
                new Variant(){
                    Name = "Assento Normal",
                    positions = true,
                    Lot = ListSimpleLotWithPosition().ToList()
                },
                new Variant(){
                    Name = "Camarote",
                    positions = true,
                    Lot = ListSimpleLotWithPosition().ToList()
                },
                new Variant(){
                    Name = "Area VIP",
                    positions = true,
                    Lot = ListSimpleLotWithPosition().ToList()
                },
            };
        }
        internal static Lot SimpleLot()
        {
            return new Lot()
            {

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
                    positions = SimplePosition()
        },
                new Lot(){
                    Description = "Lote2",
                    StartDateSales = new DateTime(2023, 07, 16, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 07, 31, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    positions = SimplePositionWithSoldPositions()
                },
                new Lot(){
                    Description = "Lote3",
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    positions = SimplePositionWithReservedPositions()
                },
                new Lot(){
                    Description = "Lote3",
                    StartDateSales = new DateTime(2023, 08, 01, 00, 00, 00),
                    EndDateSales = new DateTime(2023, 08, 15, 16, 00, 00),
                    TotalTickets = 100,
                    ValueTotal = 10000,
                    positions = SimplePositionWithReservedPositionsAndSoldPositions()
                }

            };

        }
        internal static Positions SimplePosition()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>(),
                SoldPositions = new List<int>(),
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithSoldPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>(),
                SoldPositions = new List<int>(){1,2,3},
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithReservedPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>(){4,5,6},
                SoldPositions = new List<int>(),
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithReservedPositionsAndSoldPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>(){4,5,6},
                SoldPositions = new List<int>(){1,2,3},
                TotalPositions = 100,
            };
        }
    }
}