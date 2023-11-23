namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EntranceDto
    {
        public EntranceDto()
        {
            IdColab = string.Empty;
            IdEvent = string.Empty;
            IdTicket = string.Empty;
        }

        public string IdColab { get; set; }
        public string IdEvent { get; set; }
        public string IdTicket { get; set; }
    }
}