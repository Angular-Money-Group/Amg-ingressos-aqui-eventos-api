using System.Text.Json.Serialization;

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
        [JsonPropertyName("message")]
        public string Message { get; set; }
        
        /// <summary>
        /// Objeto de dados retornado
        /// </summary>
        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}