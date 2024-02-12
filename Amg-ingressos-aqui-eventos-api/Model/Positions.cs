using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Positions
    {
        public Positions()
        {
            SoldPositions = new List<int>();
            ReservedPositions = new List<int>();
        }

        /// <summary>
        /// Total Posições
        /// </summary>
        [JsonProperty("TotalPositions")]
        public int TotalPositions { get; set; }

        /// <summary>
        /// Posições Vendidas
        /// </summary>
        [JsonProperty("SoldPositions")]
        public List<int> SoldPositions { get; set; }

        /// <summary>
        /// Posições Reservadas
        /// </summary>
        [JsonProperty("ReservedPositions")]
        public List<int> ReservedPositions { get; set; }

        /// <summary>
        /// Pessoas por Posições
        /// </summary>
        [JsonProperty("PeoplePerPositions")]
        public int PeoplePerPositions { get; set; }
    }
}