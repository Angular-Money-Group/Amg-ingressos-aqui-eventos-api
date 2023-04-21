using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Variant
    {
        /// <summary>
        /// Nome Variant
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Lista de lotes
        /// </summary>
        [Required]
        public List<Lot> Lot { get; set; }
        /// <summary>
        /// Flag Posicoes
        /// </summary>
        [Required]
        public bool positions { get; set; }
    }
}