namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class PaymentMethod
    {
        public PaymentMethod(){
            IdPaymentMethod = string.Empty;
            CardNumber = string.Empty;
            Holder = string.Empty;
            ExpirationDate = string.Empty;
            SecurityCode = string.Empty;
            Brand = string.Empty;
        }

        public string IdPaymentMethod { get; set; }
        public Enum.TypePayment TypePayment { get; set; }
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public bool SaveCard { get; set; }
        public string Brand { get; set; }
        public int Installments { get; set; }
    }
}