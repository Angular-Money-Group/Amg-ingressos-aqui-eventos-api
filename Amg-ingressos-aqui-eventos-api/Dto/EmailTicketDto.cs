using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EmailTicketDto
    {
        public EmailTicketDto()
        {
            UserName = string.Empty;
            EventName = string.Empty;
            StartDateEvent = string.Empty;
            EndDateEvent = string.Empty;
            LocalEvent = string.Empty;
            AddressEvent = string.Empty;
            VariantName = string.Empty;
            TypeTicket = string.Empty;
            UrlQrCode = string.Empty;
            Sender = string.Empty;
            To = string.Empty;
            Subject = string.Empty;
            TypeTemplate = 0;
        }
        public string UserName { get; set; }
        public string EventName { get; set; }
        public string StartDateEvent { get; set; }
        public string EndDateEvent { get; set; }
        public string LocalEvent { get; set; }
        public string AddressEvent { get; set; }
        public string VariantName { get; set; }
        public string TypeTicket { get; set; }
        public string UrlQrCode { get; set; }
        public int TypeTemplate { get; }
        public string Sender { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}