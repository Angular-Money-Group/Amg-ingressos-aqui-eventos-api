using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class FindLotsByIdVariantException : Exception
    {

        public FindLotsByIdVariantException()
        {
        }

        public FindLotsByIdVariantException(string message)
            : base(message)
        {
        }

        public FindLotsByIdVariantException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}