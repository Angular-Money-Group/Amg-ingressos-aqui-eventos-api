namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EventQrReadsDto
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Id do evento
        /// </summary>
        public string idEvent { get; set; }
        /// <summary>
        /// Id do colaborador
        /// </summary>
        public string idColab { get; set; }
        /// <summary>
        /// Total de qrcode(ticket) lidos
        /// </summary>
        public int totalReads { get; set; }
        /// <summary>
        /// Total lido com sucesso
        /// </summary>
        public int totalSuccess { get; set; }
        /// <summary>
        /// Total lido com falha
        /// </summary>
        public int totalFail { get; set; }
        /// <summary>
        /// Data inicial da leitura
        /// </summary>
        public DateTime initialDate { get; set; }
        /// <summary>
        /// Data ultima leitura
        /// </summary>
        public DateTime lastRead { get; set; }
        /// <summary>
        /// Status da leitura
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> loginHistory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> readHistory { get; set; }
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