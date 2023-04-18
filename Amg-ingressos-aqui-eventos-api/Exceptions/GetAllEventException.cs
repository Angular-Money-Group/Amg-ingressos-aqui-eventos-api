using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class GetAllEventException : Exception
    {

        public GetAllEventException()
        {
        }

        public GetAllEventException(string message)
            : base(message)
        {
        }

        public GetAllEventException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}