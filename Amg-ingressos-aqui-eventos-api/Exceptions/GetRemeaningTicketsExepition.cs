using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class GetRemeaningTicketsExepition : Exception
    {
        public GetRemeaningTicketsExepition(string message)
                  : base(message)
        {
        }
    }
}