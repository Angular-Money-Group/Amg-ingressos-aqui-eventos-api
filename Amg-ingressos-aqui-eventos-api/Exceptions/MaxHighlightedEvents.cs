using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class MaxHighlightedEvents : Exception
    {

        public MaxHighlightedEvents()
        {
        }

        public MaxHighlightedEvents(string message)
            : base(message)
        {
        }

        public MaxHighlightedEvents(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}