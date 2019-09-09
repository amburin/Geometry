using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaybillService.Application.Files;
using WaybillService.Core.Files;

namespace WaybillService.Infrastructure.Files
{
    /// <summary>
    /// This class simulates calling file storage
    /// </summary>
    public class FakeFileRepository : IFileRepository
    {
        private readonly Dictionary<CephFileId, FileWithMetadata> _files =
            new Dictionary<CephFileId, FileWithMetadata>();

        public Task<CephFileId> Save(FileWithMetadata fileWithMetadata)
        {
            var rawGuid = Guid.NewGuid();
            var fileId = new CephFileId(rawGuid);

            _files.Add(fileId, fileWithMetadata);

            return Task.FromResult(fileId);
        }

        public Task<FileWithMetadata> Get(CephFileId cephFileId)
        {
            var file = _files[cephFileId];
            return Task.FromResult(file);
        }
    }
}