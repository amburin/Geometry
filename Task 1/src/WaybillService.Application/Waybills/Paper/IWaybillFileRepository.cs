using System.Threading.Tasks;
using WaybillService.Application.Waybills.Paper.FileImport;
using WaybillService.Core.Files;

namespace WaybillService.Application.Waybills.Paper
{
    public interface IWaybillFileRepository
    {
        Task<CephFileId> Save(WaybillFile waybillFile);
        Task<WaybillFile> Get(CephFileId cephFileId);
    }
}