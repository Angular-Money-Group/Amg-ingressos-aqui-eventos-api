using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class TicketUserDto : Ticket
    {
        public TicketUserDto()
        {
            User = new UserDto();
        }
        
        public UserDto User { get; set; }
    }
}