using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{

    public partial class GetEventsWithNames : Event
    {
        public GetEventsWithNames()
        {
            User = new Producer();
        }

        [JsonProperty("User")]
        public Producer User { get; set; }
    };

    public class Producer
    {
        public Producer()
        {
            Name = string.Empty;
        }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}