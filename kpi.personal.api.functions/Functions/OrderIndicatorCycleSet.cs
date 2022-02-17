using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using kpi.personal.api.functions.Model.Events;
using kpi.personal.api.functions.Services;

namespace kpi.personal.api.functions.Functions
{
    public class OrderIndicatorCycleSet : BaseEventFunction
    {
        [FunctionName("OrderIndicatorCycleSet")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "OrderIndicatorCycleSet")] HttpRequest req,
            ILogger log)
        {
            try
            {
                // Reads body content
                string data = await new StreamReader(req.Body).ReadToEndAsync();

                // Validades JWT and gets payload
                string payload = ValidateJWT(data);

                // Deserializes the payload
                OrderEventArgs orderEventArgs = JsonConvert.DeserializeObject<OrderEventArgs>(payload);

                // Validates the event
                // Validates the event
                bool cancel;
                if (orderEventArgs.Sub == "OrderIndicatorCycleSet") cancel = false;
                else if (orderEventArgs.Sub == "OrderIndicatorCancelCycleSet") cancel = true;
                else throw new ApplicationException("InvalidEventSubject");

                // Updates kpis
                await OrderService.CreateKpiEventAsync(orderEventArgs, cancel);

                // Returns no content status code
                return new NoContentResult();
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
