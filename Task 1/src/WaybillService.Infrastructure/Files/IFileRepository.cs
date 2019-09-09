using System.Threading.Tasks;
using WaybillService.Application.Files;
using WaybillService.Core.Files;

namespace WaybillService.Infrastructure.Files
{
    public interface IFileRepository
    {
        Task<CephFileId> Save(FileWithMetadata fileWithMetadata);
        Task<FileWithMetadata> Get(CephFileId cephFileId);
    }
}