namespace Amg_ingressos_aqui_eventos_api.Infra
{
    public class EventDatabaseSettings
    {
        /// <summary>
        /// Connection string base de dados Mongo
        /// </summary>
        public string ConnectionString { get; set; } = null!;
        /// <summary>
        /// Nome base de dados Mongo
        /// </summary>
        public string DatabaseName { get; set; } = null!;
        /// <summary>
        /// Nome collection Mongo
        /// </summary>
        public string EventCollectionName { get; set; } = null!;
    }
}