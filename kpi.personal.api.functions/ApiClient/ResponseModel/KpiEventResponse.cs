using System;
using Newtonsoft.Json;

namespace kpi.personal.api.functions.ApiClient.ResponseModel
{
    public class KpiEventResponse
    {
        [JsonProperty("sellerCode")]
        public int SellerCode { get; set; }

        [JsonProperty("kpiCode")]
        public int KpiCode { get; set; }

        [JsonProperty("cycle")]
        public int Cycle { get; set; }

        [JsonProperty("oldAmountRealized")]
        public decimal? OldAmountRealized { get; set; }

        [JsonProperty("amountRealized")]
        public decimal? AmountRealized { get; set; }

        [JsonProperty("updateDate")]
        public long UpdateDate { get; set; }
    }
}
