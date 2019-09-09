using System.Threading.Tasks;

namespace WaybillService.Core.Waybills
{
    public interface IWaybillRepository
    {
        Task<Waybill> Get(WaybillId waybillId);
    }
}