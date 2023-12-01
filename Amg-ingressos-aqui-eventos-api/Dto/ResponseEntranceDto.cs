namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class ResponseEntranceDto
    {
        public ResponseEntranceDto()
        {
            UserName = string.Empty;
            UserDoc = string.Empty;
            UserEmail = string.Empty;
            VariantName = string.Empty;
        }

        public bool ReqDocs { get; set; }
        public string UserName { get; set; }
        public string UserDoc { get; set; }
        public string UserEmail { get; set; }
        public string VariantName { get; set; }
    }
}