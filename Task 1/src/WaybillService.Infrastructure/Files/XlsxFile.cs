using WaybillService.Application;

namespace WaybillService.Infrastructure.Files
{
    public class XlsxFile : File
    {
        public override string ContentType =>
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public XlsxFile(string name, byte[] bytes) : base(name, bytes)
        {
        }
    }
}