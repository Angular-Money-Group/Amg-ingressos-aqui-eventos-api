using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{

    public partial class GetEventsWithNames : Event
    {
        [JsonProperty("User")]
        public Producer User { get; set; }
    };

    public class Producer
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}