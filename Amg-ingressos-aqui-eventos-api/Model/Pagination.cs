using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Pagination
    {
        /// <summary>
        /// Pagina atual
        /// </summary>
        [Required]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Tamanho da Pagina
        /// </summary>
        [Required]
        public int PageSize { get; set; } = 10;
    }
}