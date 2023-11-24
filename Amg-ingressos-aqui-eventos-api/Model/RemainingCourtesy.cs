using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class RemainingCourtesy
    {
        public RemainingCourtesy()
        {
            VariantName = string.Empty;
            VariantId = string.Empty;
        }
        
        /// <summary>
        /// Quantidade de cortesias faltantes
        /// </summary>
        [BsonElement("Quantity")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Nome da Variante
        /// </summary>
        [BsonElement("VariantName")]
        [JsonPropertyName("variantName")]
        public string VariantName { get; set; }

        /// <summary>
        /// Id da Variante
        /// </summary>
        [BsonElement("VariantId")]
        [JsonPropertyName("variantId")]
        public string VariantId { get; set; }
    }
}