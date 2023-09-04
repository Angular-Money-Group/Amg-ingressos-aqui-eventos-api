using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class ReportTransactionsDto
    {
        public string _id { get; set; }
        public string IdPerson { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Tax { get; set; }
        public string TotalValue { get; set; }
        public string Discount { get; set; }
        public int Status { get; set; }
        public int Stage { get; set; }
        public object ReturnUrl { get; set; }
        public string PaymentIdService { get; set; }
        public string Details { get; set; }
        public int TotalTicket { get; set; }
        public DateTime DateRegister { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
    public class PaymentMethod
    {
        public object IdPaymentMethod { get; set; }
        public int TypePayment { get; set; }
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public bool SaveCard { get; set; }
        public string Brand { get; set; }
        public int Installments { get; set; }
    }

    public class Transaction
    {
        public string _id { get; set; }
        public string IdPerson { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Tax { get; set; }
        public string TotalValue { get; set; }
        public string Discount { get; set; }
        public int Status { get; set; }
        public int Stage { get; set; }
        public object ReturnUrl { get; set; }
        public string PaymentIdService { get; set; }
        public string Details { get; set; }
        public int TotalTicket { get; set; }
        public DateTime DateRegister { get; set; }
    }
}