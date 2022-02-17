using System;
using Newtonsoft.Json;

namespace kpi.personal.api.functions.ApiClient.RequestModel
{
    public class KpiEventRequest
    {
        [JsonProperty("eventGuid")]
        public Guid EventGuid { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventDate")]
        public long EventDate { get; set; }

        [JsonProperty("eventEntityCode")]
        public int? EventEntityCode { get; set; }

        [JsonProperty("eventEntityAdditionalCode")]
        public int? EventEntityAdditionalCode { get; set; }

        [JsonProperty("cycle")]
        public int Cycle { get; set; }

        [JsonProperty("amountRealized")]
        public decimal AmountRealized { get; set; }

        [JsonProperty("incremental")]
        public bool Incremental { get; set; } = true;

        [JsonProperty("lastUpdateDate")]
        public long? LastUpdateDate { get; set; }
    }
}
