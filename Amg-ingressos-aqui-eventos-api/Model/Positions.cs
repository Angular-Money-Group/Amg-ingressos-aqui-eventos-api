using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Positions
    {
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
    }
}