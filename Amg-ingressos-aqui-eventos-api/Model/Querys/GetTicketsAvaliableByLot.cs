using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public partial class GetTicketsAvaliableByLot
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("TotalTickets")]
        public long TotalTickets { get; set; }

        [JsonProperty("ValueTotal")]
        public long ValueTotal { get; set; }

        [JsonProperty("StartDateSales")]
        public string StartDateSales { get; set; }

        [JsonProperty("EndDateSales")]
        public string EndDateSales { get; set; }

        [JsonProperty("Positions")]
        public PositionsLot Positions { get; set; }

        [JsonProperty("Status")]
        public long Status { get; set; }

        [JsonProperty("IdVariant")]
        public string IdVariant { get; set; }

        [JsonProperty("tickets")]
        public TicketLot[] Tickets { get; set; }
    }

    public partial class PositionsLot
    {
        [JsonProperty("TotalPositions")]
        public long TotalPositions { get; set; }

        [JsonProperty("SoldPositions")]
        public long[] SoldPositions { get; set; }

        [JsonProperty("ReservedPositions")]
        public long[] ReservedPositions { get; set; }
    }

    public partial class TicketLot
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("IdLot")]
        public string IdLot { get; set; }

        [JsonProperty("Position")]
        public object Position { get; set; }

        [JsonProperty("Value")]
        public long Value { get; set; }

        [JsonProperty("IdUser")]
        public string IdUser { get; set; }


    }
}