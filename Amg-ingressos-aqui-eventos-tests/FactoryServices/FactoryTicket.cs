using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryTicket
    {
        internal static Ticket SimpleTicket()
        {
            return new Ticket()
            {
                IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                Position = "0",
                Value = new decimal(150),
                IdUser = "3b241101-e2bb-4255-8caf-4136c566a962",
            };
        }

        internal static IEnumerable<Ticket> ListSimpleTicket()
        {
            return new List<Ticket>(){
                new Ticket(){
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdUser = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = string.Empty,
                    Value = new decimal(150)
                },
                new Ticket(){
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdUser = "3b241101-e2bb-4255-8caf-4136c566a962",

                    Position = string.Empty,
                    Value = new decimal(200)
                },
                new Ticket(){
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdUser = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = string.Empty,
                    Value = new decimal(300)
                }
            };
        }

        internal static IEnumerable<Ticket> ListSimpleTicketWithoutIdUser()
        {
            return new List<Ticket>(){
                new Ticket(){
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdUser = string.Empty,
                    Position = string.Empty,
                    Value = new decimal(150)
                },
                new Ticket(){
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdUser = string.Empty,
                    Position = string.Empty,
                    Value = new decimal(200)
                },
                new Ticket(){
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdUser = string.Empty,
                    Position = string.Empty,
                    Value = new decimal(300)
                }
            };
        }

        internal static IEnumerable<Ticket> ListSimpleTicketWithPosition()
        {
            return new List<Ticket>(){
                new Ticket(){
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdUser = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = "1",
                    Value = new decimal(150)
                },
                new Ticket(){
                    IdUser = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = "2",
                    Value = new decimal(200)
                },
                new Ticket(){
                    IdUser = "3b241101-e2bb-4255-8caf-4136c566a962",
                    IdLot = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = "3",
                    Value = new decimal(300)
                }
            };
        }
    }
}