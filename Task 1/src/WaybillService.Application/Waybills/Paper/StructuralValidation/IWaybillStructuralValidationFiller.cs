using System.Threading.Tasks;
using WaybillService.Core;
using WaybillService.Core.Waybills;

namespace WaybillService.Application.Waybills.Paper.StructuralValidation
{
    public interface IWaybillStructuralValidationFiller
    {
        Task FillStructuralValidation(WaybillId waybillId);
    }
}