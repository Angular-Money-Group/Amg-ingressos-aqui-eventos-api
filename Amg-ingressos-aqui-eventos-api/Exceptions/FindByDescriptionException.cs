using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class FindByDescriptionException : Exception
    {

        public FindByDescriptionException()
        {
        }

        public FindByDescriptionException(string message)
            : base(message)
        {
        }

        public FindByDescriptionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}