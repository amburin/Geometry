using System.IO;

namespace WaybillService.Application.Files
{
    public interface IExcelFileFactory
    {
        File Create(string nameWithExtension, Stream content);
    }
}