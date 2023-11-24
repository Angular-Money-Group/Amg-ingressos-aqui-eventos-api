using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{

    public partial class GetEvents : Event
    {
        [JsonProperty("Colabs")]
        public List<string>? Colabs { get; set; }

    }
}