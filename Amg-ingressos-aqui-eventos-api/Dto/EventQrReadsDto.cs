namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EventQrReadsDto
    {
        public EventQrReadsDto()
        {
            Id = string.Empty;
            IdEvent = string.Empty;
            IdColab = string.Empty;
            NameUser = string.Empty;
            LoginHistory = new List<string>();
            ReadHistory = new List<string>();
            DocumentId = string.Empty;
            NameVariant = string.Empty;
        }
        
        /// <summary>
        /// Id mongo
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Id do evento
        /// </summary>
        public string IdEvent { get; set; }
        /// <summary>
        /// Id do colaborador
        /// </summary>
        public string IdColab { get; set; }
        /// <summary>
        /// Total de qrcode(ticket) lidos
        /// </summary>
        public int TotalReads { get; set; }
        /// <summary>
        /// Total lido com sucesso
        /// </summary>
        public int TotalSuccess { get; set; }
        /// <summary>
        /// Total lido com falha
        /// </summary>
        public int TotalFail { get; set; }
        /// <summary>
        /// Data inicial da leitura
        /// </summary>
        public DateTime InitialDate { get; set; }
        /// <summary>
        /// Data ultima leitura
        /// </summary>
        public DateTime LastRead { get; set; }
        /// <summary>
        /// Status da leitura
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> LoginHistory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> ReadHistory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NameUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DocumentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NameVariant { get; set; } 
    }
}