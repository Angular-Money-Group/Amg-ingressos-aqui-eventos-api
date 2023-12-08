namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class ReadHistoryDto
    {
        public ReadHistoryDto()
        {
            Id = string.Empty;
            IdEvent = string.Empty;
            IdColab = string.Empty;
            IdTicket = string.Empty;
            Reason = string.Empty;
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
        /// Id Colab que leu o ticket
        /// </summary>
        public string IdColab { get; set; }
        /// <summary>
        /// Id do ticket (qrcode)
        /// </summary>
        public string IdTicket { get; set; }

        /// <summary>
        /// Data leitura
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Status do ticket
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Observação leitura
        /// </summary>
        public string Reason { get; set; }

    }
}