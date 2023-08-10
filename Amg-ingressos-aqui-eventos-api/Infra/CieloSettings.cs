namespace Amg_ingressos_aqui_eventos_api.Infra
{
    public class CieloSettings
    {
        /// <summary>
        /// Connection string base de dados Mongo
        /// </summary>
        public string MerchantIdHomolog { get; set; } = null!;
        /// <summary>
        /// Nome base de dados Mongo
        /// </summary>
        public string MerchantKeyHomolog { get; set; } = null!;
        /// <summary>
        /// Nome collection Mongo
        /// </summary>
        public string UrlApiHomolog { get; set; } = null!;
    }
}