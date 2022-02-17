using System;

using kpi.personal.api.functions.Model.Events;
using kpi.personal.api.functions.ApiClient.RequestModel;

namespace kpi.personal.api.functions.Services
{
    public class BaseService
    {
        // kpis codes
        protected const int IndicadorPessoalPedidos = 1000;
        protected const int IndicadorPessoalFaturamento = 1001;
        protected const int IndicadorPessoalPontos = 1002;

        static BaseService()
        {
        }

        protected static KpiEventRequest CreateKpiEventRequest(BaseEventArgs eventArgs)
        {
            return new KpiEventRequest()
            {
                EventGuid = eventArgs.Jti,
                EventType = eventArgs.Sub,
                EventDate = eventArgs.Iat
            };
        }
    }
}
