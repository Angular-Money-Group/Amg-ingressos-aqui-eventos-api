using System.Text.Json.Serialization;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class CardDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("month")]
        public string Month { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        public CardDto()
        {
            Id = string.Empty;
            Name = string.Empty;
            Day = string.Empty;
            Month = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Description = string.Empty;
            Image = string.Empty;
            Year = string.Empty;
        }
        public List<CardDto> ModelListToDtoList(List<EventComplet> listEvent)
        {
            return listEvent.Select(ModelToDto).ToList();
        }
        public CardDto ModelToDto(EventComplet eventData)
        {
            return new CardDto()
            {
                Name = eventData.Name,
                Id = eventData.Id,
                Day = eventData.StartDate.Day.ToString(),
                Month = eventData.StartDate.Month.ToString(),
                Year = eventData.StartDate.Year.ToString(),
                City = eventData.Address.City,
                State = eventData.Address.State,
                Description = eventData.Description,
                Image = eventData.Image
            };
        }
    }
}