using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryEvent
    {
        internal static Event SimpleEvent()
        {
            return new Event()
            {
                Name = "teste name",
                Local = "Uberlandia",
                Type = "1",
                Image = "teste",
                Description = "teste3",
                Cep = "38400000",
                Address = "Parque Sabiá",
                Number = 1,
                Neighborhood = "teste",
                Complement = "teste",
                ReferencePoint = "n/a",
                City = "Uberlandia",
                State = "MG",
                Day = "15",
                Lot = "1",
                VipArea = "1"
            };
        }

        internal static IEnumerable<Event> ListSimpleEvent()
        {
            List<Event> listEvent = new List<Event>();
            listEvent.Add(
                new Event()
                {
                    Name = "teste name1",
                    Local = "Uberlandia",
                    Type = "1",
                    Image = "teste1",
                    Description = "teste1",
                    Cep = "38400000",
                    Address = "Parque Sabiá",
                    Number = 1,
                    Neighborhood = "teste1",
                    Complement = "teste1",
                    ReferencePoint = "n/a",
                    City = "Uberlandia",
                    State = "MG",
                    Day = "15",
                    Lot = "1",
                    VipArea = "1"
                });
            listEvent.Add(
                new Event()
                {
                    Name = "teste name2",
                    Local = "Uberlandia",
                    Type = "1",
                    Image = "teste 2",
                    Description = "teste2",
                    Cep = "38400000",
                    Address = "Parque Sabiá",
                    Number = 2,
                    Neighborhood = "teste2",
                    Complement = "teste2",
                    ReferencePoint = "n/a",
                    City = "Uberlandia",
                    State = "MG",
                    Day = "31",
                    Lot = "2",
                    VipArea = "2"
                });
                
            return listEvent;
        }
    }
}