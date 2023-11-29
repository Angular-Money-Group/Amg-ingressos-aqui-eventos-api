using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class RuleException: Exception
    {
        public RuleException()
        {
        }

        public RuleException(string message)
            : base(message)
        {
        }

        public RuleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}