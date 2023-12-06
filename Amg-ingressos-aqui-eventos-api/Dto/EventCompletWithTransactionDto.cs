using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EventCompletWithTransactionDto : EventCompletDto
    {
        public EventCompletWithTransactionDto()
        {
            Transactions = new List<Transaction>();
        }
        public List<Transaction> Transactions { get; set; }
    }
}