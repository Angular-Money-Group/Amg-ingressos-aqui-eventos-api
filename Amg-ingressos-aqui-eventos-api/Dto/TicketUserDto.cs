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

        internal IEnumerable<TicketUserDto> ModelListToDtoList(List<TicketComplet> data)
        {
            return data.Select(t => ModelToDto(t));
        }
        internal TicketUserDto ModelToDto(TicketComplet data)
        {
            return new TicketUserDto()
            {
                Id = data.Id,
                IdColab = data.IdColab,
                IdLot = data.IdLot,
                IdUser = data.IdUser,
                IsSold = data.IsSold,
                Position = data.Position,
                QrCode = data.QrCode,
                ReqDocs = data.ReqDocs,
                Status = data.Status,
                TicketCortesia = data.TicketCortesia,
                Value = data.Value,
                User = new UserDto().ModelToDto(data.Users.Find(u => u.Id == data.IdUser) ?? new User())
            };
        }
    }
}