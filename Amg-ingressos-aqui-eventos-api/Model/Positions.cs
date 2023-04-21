using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Positions
    {
        /// <summary>
        /// Total Posições
        /// </summary>
        [Required]
        public int TotalPositions { get; set; }
        /// <summary>
        /// Posições Vendidas
        /// </summary>
        public List<int> SoldPositions { get; set; }
        /// <summary>
        /// Posições Reservadas
        /// </summary>
        public List<int> ReservedPositions { get; set; }
    }
}