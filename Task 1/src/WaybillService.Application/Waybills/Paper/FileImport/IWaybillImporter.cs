using System.Collections.Generic;
using System.Threading.Tasks;
using WaybillService.Core;
using WaybillService.Core.Waybills;

namespace WaybillService.Application.Waybills.Paper.FileImport
{
    public interface IWaybillImporter
    {
        Task ImportFromFiles(
            OrderId orderId,
            IReadOnlyCollection<File> files);
    }
}