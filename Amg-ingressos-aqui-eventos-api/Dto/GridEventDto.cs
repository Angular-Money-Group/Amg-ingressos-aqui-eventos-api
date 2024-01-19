using System.Text.Json.Serialization;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class GridEventDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("local")]
        public string Local { get; set; }

        public GridEventDto()
        {
            Id = string.Empty;
            Name = string.Empty;
            StartDate = string.Empty;
            EndDate = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Description = string.Empty;
            Image = string.Empty;
            Type = string.Empty;
            Local = string.Empty;
        }
        public List<GridEventDto> ModelListToDtoList(List<EventComplet> listEvent)
        {
            return listEvent.Select(ModelToDto).ToList();
        }
        public GridEventDto ModelToDto(EventComplet eventData)
        {
            return new GridEventDto()
            {
                Name = eventData.Name,
                Id = eventData.Id,
                EndDate = eventData.EndDate.ToString(),
                StartDate = eventData.StartDate.ToString(),
                City = eventData.Address.City,
                State = eventData.Address.State,
                Description = eventData.Description,
                Image = eventData.Image,
                Local = eventData.Local,
                Type = eventData.Type
            };
        }
    }
}