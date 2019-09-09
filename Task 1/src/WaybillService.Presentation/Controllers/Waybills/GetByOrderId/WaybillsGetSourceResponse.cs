using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WaybillService.Presentation.Controllers.Waybills.GetByOrderId
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WaybillsGetSourceResponse
    {
        Edo,
        Paper
    }
}