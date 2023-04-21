using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Lot
    {
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        public string? Description { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        public int TotalTickets { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        public decimal ValueTotal { get; set; }
        /// <summary>
        /// Data Inicio das vendas
        /// </summary>
        [Required]
        public DateTime StartDateSales { get; set; }
        /// <summary>
        /// Data Fim das vendas
        /// </summary>
        [Required]
        public DateTime EndDateSales { get; set; }
        /// <summary>
        /// Data Fim das vendas
        /// </summary>
        public Positions positions { get; set; }

    }
}