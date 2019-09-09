using WaybillService.Application;

namespace WaybillService.Infrastructure.Files
{
    public class XlsFile : File
    {
        public override string ContentType =>
            "application/vnd.ms-excel";

        public XlsFile(string name, byte[] bytes) : base(name, bytes)
        {
        }
    }
}