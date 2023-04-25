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
                Variant = FactoryVariant.ListSimpleVariant().ToList(),
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
                Variant = FactoryVariant.ListSimpleVariantWithPosition().ToList(),
            };
        }
        internal static IEnumerable<Event> ListSimpleEvent()
        {
            List<Event> listEvent = new List<Event>();
            listEvent.Add(SimpleEvent());
            listEvent.Add(SimpleEventWithPosition());

            return listEvent;
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
                SoldPositions = new List<int>() { 1, 2, 3 },
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithReservedPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>() { 4, 5, 6 },
                SoldPositions = new List<int>(),
                TotalPositions = 100,
            };
        }
        internal static Positions SimplePositionWithReservedPositionsAndSoldPositions()
        {
            return new Positions()
            {
                ReservedPositions = new List<int>() { 4, 5, 6 },
                SoldPositions = new List<int>() { 1, 2, 3 },
                TotalPositions = 100,
            };
        }
    }
}