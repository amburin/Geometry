using System.Threading.Tasks;
using WaybillService.Core;
using WaybillService.Core.Waybills;

namespace WaybillService.Application.Waybills.Paper.ContentFilling
{
    public interface IWaybillContentFiller
    {
        Task FillContent(WaybillId waybillId);
    }
}