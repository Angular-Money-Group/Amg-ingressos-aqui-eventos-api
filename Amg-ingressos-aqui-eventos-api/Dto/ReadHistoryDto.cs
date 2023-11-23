namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class ReadHistoryDto
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
        /// Id Colab que leu o ticket
        /// </summary>
        public string idColab { get; set; }
        /// <summary>
        /// Id do ticket (qrcode)
        /// </summary>
        public string idTicket { get; set; }

        /// <summary>
        /// Data leitura
        /// </summary>
        public DateTime date { get; set; }
        /// <summary>
        /// Status do ticket
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// Observação leitura
        /// </summary>
        public string reason { get; set; }

    }
}