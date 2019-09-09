using System;
using System.IO;
using WaybillService.Application.Files;
using File = WaybillService.Application.File;

namespace WaybillService.Infrastructure.Files
{
    public class ExcelFileFactory : IExcelFileFactory
    {
        public File Create(string nameWithExtension, Stream content)
        {
            if (nameWithExtension.EndsWith(".xlsx", StringComparison.InvariantCulture))
            {
                return new XlsxFile(nameWithExtension, content.ToByteArray());
            }

            if (nameWithExtension.EndsWith(".xls", StringComparison.InvariantCulture))
            {
                return new XlsFile(nameWithExtension, content.ToByteArray());
            }

            throw new ArgumentException(
                $"Cannot determine excel format for name {nameWithExtension}");
        }
    }
}