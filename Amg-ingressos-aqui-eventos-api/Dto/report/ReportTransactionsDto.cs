using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class ReportTransactionsDto
    {
        public ReportTransactionsDto(){
            Credito = new TransactionsDto();
            Debito = new TransactionsDto();
            Pix = new TransactionsDto();
            Total = new TransactionsDto();
        }
        public TransactionsDto Credito { get; set; }
        public TransactionsDto Debito { get; set; }
        public TransactionsDto Pix { get; set; }
        public TransactionsDto Total { get; set; }
    }
    public class TransactionsDto
    {
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
        public double ValorTaxas { get; set; }
        public double ValorEvento { get; set; }
        public double ValorLiquido { get; set; }

    }
}