using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class TicketUserDto
    {
        public string? Id { get; set; }
        public string? IdLot { get; set; }
        public string? IdUser { get; set; }
        public string? Position { get; set; }
        public decimal Value { get; set; }
        public bool ReqDocs { get; set; }
        public Enum.StatusTicket Status { get; set; }
        public string QrCode { get; set; }
        public UserDto User { get; set; }
        
    }
}