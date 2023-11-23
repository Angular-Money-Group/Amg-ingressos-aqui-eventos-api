namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EntranceDto
    {
        public string IdColab { get; set; } = string.Empty;
        public string IdEvent { get; set; } = string.Empty;
        public string IdTicket { get; set; } = string.Empty;
    }

    public class ResponseEntranceDTO
    {
        public Boolean reqDocs { get; set; }
        public string userName { get; set; } = string.Empty;
        public string userDoc { get; set; } = string.Empty;
        public string userEmail { get; set; } = string.Empty;
        public string variantName { get; set; } = string.Empty;
    }
}
