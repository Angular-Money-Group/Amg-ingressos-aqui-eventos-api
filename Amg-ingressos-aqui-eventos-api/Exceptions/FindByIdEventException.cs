using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class FindByIdEventException : Exception
    {

        public FindByIdEventException()
        {
        }

        public FindByIdEventException(string message)
            : base(message)
        {
        }

        public FindByIdEventException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}