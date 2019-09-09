using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using WaybillService.Core.Files;

namespace WaybillService.Application.Waybills.Paper.FileImport
{
    public class WaybillFile
    {
        public CephFileId Id { get; protected set; }
        public DateTimeOffset UploadTime { get; }

        private File File { get; }

        public string Name => File.Name;
        public string ContentType => File.ContentType;
        public ReadOnlyCollection<byte> Bytes => File.Bytes;

        public WaybillFile(File file, DateTimeOffset uploadTime)
        {
            File = file;
            UploadTime = uploadTime;
        }

        public MemoryStream CreateFileStream() => new MemoryStream(Bytes.ToArray());
    }
}