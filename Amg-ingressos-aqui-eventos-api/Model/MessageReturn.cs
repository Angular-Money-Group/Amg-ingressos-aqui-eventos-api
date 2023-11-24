namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class MessageReturn
    {
        public MessageReturn()
        {
            Message = string.Empty;
            Data = string.Empty;
        }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Objeto de dados retornado
        /// </summary>
        public object Data { get; set; }
    }
}