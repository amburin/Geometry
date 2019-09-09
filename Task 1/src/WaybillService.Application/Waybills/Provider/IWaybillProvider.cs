using System.Collections.Generic;
using System.Threading.Tasks;
using WaybillService.Core;
using WaybillService.Core.Waybills;

namespace WaybillService.Application.Waybills.Provider
{
    public interface IWaybillProvider
    {
        Task<IReadOnlyList<GetWaybillsResult>> Get(OrderId orderId);
    }
}