using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaybillService.Application.Files;
using WaybillService.Application.Waybills.Paper;
using WaybillService.Application.Waybills.Paper.FileImport;
using WaybillService.Core.Files;
using WaybillService.Database.Context;
using WaybillService.Database.Context.Waybills.Models;
using WaybillService.Infrastructure.Files;
using File = WaybillService.Application.File;
using FileWithMetadata = WaybillService.Application.Files.FileWithMetadata;

namespace WaybillService.Infrastructure.Waybills
{
    public class WaybillFileRepository : IWaybillFileRepository
    {
        private readonly IFileRepository _fileRepository;
        private readonly WaybillContext _context;
        private readonly IExcelFileFactory _excelFileFactory;

        private readonly string[] _tags = {"supplier", "waybills"};

        public WaybillFileRepository(
            WaybillContext context,
            IFileRepository fileRepository,
            IExcelFileFactory excelFileFactory)
        {
            _fileRepository = fileRepository;
            _excelFileFactory = excelFileFactory;
            _context = context;
        }

        public async Task<WaybillFile> Get(CephFileId cephFileId)
        {
            var file = await _fileRepository.Get(cephFileId);

            var waybillFileDto =
                await _context.WaybillFiles.SingleOrDefaultAsync(
                    x => x.CephId == cephFileId.ToGuid());

            var excelFile = _excelFileFactory.Create(
                waybillFileDto.Name,
                new MemoryStream(file.FileContent.ToArray()));

            return new WaybillFileWithId(cephFileId, excelFile, waybillFileDto.UploadTime);
        }

        private class WaybillFileWithId : WaybillFile
        {
            public WaybillFileWithId(CephFileId cephFileId, File file, DateTimeOffset uploadTime)
                : base(file, uploadTime)
            {
                Id = cephFileId;
            }
        }

        public async Task<CephFileId> Save(WaybillFile waybillFile)
        {
            var fileWithMetadata = new FileWithMetadata(
                filename: CreateName(waybillFile),
                contentType: waybillFile.ContentType,
                tags: _tags,
                fileContent: waybillFile.Bytes
            );

            var fileId = await _fileRepository.Save(fileWithMetadata);

            var waybillFileDto = new WaybillFileDto
            {
                CephId = fileId.ToGuid(),
                Name = waybillFile.Name,
                UploadTime = waybillFile.UploadTime,
            };

            await _context.WaybillFiles.AddAsync(waybillFileDto);
            await _context.SaveChangesAsync();

            return fileId;
        }

        private static string CreateName(WaybillFile waybillFile)
        {
            var uploadTimeTicks = waybillFile.UploadTime.Ticks;
            return $"{Guid.NewGuid():D}-{uploadTimeTicks:D}";
        }
    }
}