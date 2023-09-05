using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Enum
{
    public enum StageTransactionEnum
    {
        Confirm = 0,
        PersonData = 1,
        TicketsData = 2,
        PaymentData = 3,
        PaymentTransaction = 4,
        Finished = 5
    }
}