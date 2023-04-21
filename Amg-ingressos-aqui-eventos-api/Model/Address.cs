using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Address
    {
        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        public string Cep { get; set; }
        /// <summary>
        /// Endereço
        /// </summary>
        [Required]
        public string AddressDescription { get; set; }
        /// <summary>
        /// Número
        /// </summary>
        [Required]
        public string Number { get; set; }
        /// <summary>
        /// Vizinho
        /// </summary>
        [Required]
        public string Neighborhood { get; set; }
        /// <summary>
        /// Complemento
        /// </summary>
        [Required]
        public string Complement { get; set; }
        /// <summary>
        /// Ponto de referencia
        /// </summary>
        [Required]
        public string ReferencePoint { get; set; }
        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string City { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        public string State { get; set; }
        
    }
}