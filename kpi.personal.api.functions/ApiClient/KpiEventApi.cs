using System;
using System.Threading.Tasks;

using kpi.personal.api.functions.ApiClient.RequestModel;
using kpi.personal.api.functions.ApiClient.ResponseModel;

namespace kpi.personal.api.functions.ApiClient
{
    public class KpiEventApi : BaseApiClient
    {
        private KpiEventApi()
        {
        }

        public static async Task<ApiResponse<KpiEventResponse>> PostKpiEventAsync(int representativeCode, int kpiCode, KpiEventRequest kpiEvent)
        {
            return await PostAsync<KpiEventResponse, KpiEventRequest>(string.Format("sellers/{0}/kpis/{1}/event", representativeCode, kpiCode), kpiEvent);
        }
    }
}
