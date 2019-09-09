using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WaybillService.Core.Files;

namespace WaybillService.Application.Files
{
    public class FileWithMetadata
    {
        public FileWithMetadata(
            string filename,
            string contentType,
            string[] tags,
            IList<byte> fileContent)
        {
            Filename = filename;
            ContentType = contentType;
            Tags = tags;
            FileContent = new ReadOnlyCollection<byte>(fileContent);
        }

        public FileWithMetadata(
            Guid fileId,
            string filename,
            string contentType,
            IReadOnlyCollection<string> tags,
            DateTimeOffset? createdAt,
            byte[] fileContent)
        {
            CephFileId = new CephFileId(fileId);
            Filename = filename;
            ContentType = contentType;
            Tags = tags;
            CreatedAt = createdAt;
            FileContent = new ReadOnlyCollection<byte>(fileContent);
        }

        public CephFileId CephFileId { get; }
        public string Filename { get; }
        public string ContentType { get; }
        public IReadOnlyCollection<string> Tags { get; }
        public DateTimeOffset? CreatedAt { get; }
        public ReadOnlyCollection<byte> FileContent { get; }
        public long SizeInBytes => FileContent.Count;
    }
}