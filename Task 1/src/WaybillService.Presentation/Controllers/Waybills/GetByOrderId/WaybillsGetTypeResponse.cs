using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WaybillService.Presentation.Controllers.Waybills.GetByOrderId
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WaybillsGetTypeResponse
    {
        Utd1,
        Utd2,
        Invoice,
        ConsignmentNote
    }
}