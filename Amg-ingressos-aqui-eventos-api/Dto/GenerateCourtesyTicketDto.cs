namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class CourtesyTicketDto
    {
        public CourtesyTicketDto(){
            Email = string.Empty;
            IdVariant = string.Empty;
        }

        public string Email { get; set; }
        public string IdVariant { get; set; }
        public int Quantity { get; set; }
    }
}