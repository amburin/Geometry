using System.Threading.Tasks;

namespace WaybillService.Core.Waybills.Paper
{
    public interface IPaperWaybillRepository
    {
        Task<WaybillId> Add(PaperWaybill waybill);
        Task<PaperWaybill> Get(WaybillId waybillId);
        Task Update(PaperWaybill waybill);
    }
}