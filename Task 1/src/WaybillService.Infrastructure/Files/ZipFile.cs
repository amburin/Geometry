using WaybillService.Application;

namespace WaybillService.Infrastructure.Files
{
    public class ZipFile : File
    {
        public ZipFile(string name, byte[] bytes) : base(name, bytes)
        {
        }

        public override string ContentType => "application/zip";
    }
}