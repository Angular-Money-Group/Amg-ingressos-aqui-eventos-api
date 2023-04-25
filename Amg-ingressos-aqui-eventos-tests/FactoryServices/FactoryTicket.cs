using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_tests.FactoryServices
{
    public static class FactoryTicket
    {
        internal static Ticket SimpleTicket()
        {
            return new Ticket()
            {
                IdLote = "3b241101-e2bb-4255-8caf-4136c566a962",
                Position = "0",
                Value = new decimal(150),
            };
        }
        internal static IEnumerable<Ticket> ListSimpleTicket()
        {
            return new List<Ticket>(){
                new Ticket(){
                    IdLote = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = string.Empty,
                    Value = new decimal(150)
                },
                new Ticket(){
                    IdLote = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = string.Empty,
                    Value = new decimal(200)
                },
                new Ticket(){
                    IdLote = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = string.Empty,
                    Value = new decimal(300)
                }
            };
        }
        internal static IEnumerable<Ticket> ListSimpleTicketWithPosition()
        {
            return new List<Ticket>(){
                new Ticket(){
                    IdLote = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = "1",
                    Value = new decimal(150)
                },
                new Ticket(){
                    IdLote = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = "2",
                    Value = new decimal(200)
                },
                new Ticket(){
                    IdLote = "3b241101-e2bb-4255-8caf-4136c566a962",
                    Position = "3",
                    Value = new decimal(300)
                }
            };
        }
    }
}