using System.IO;
using System.Threading.Tasks;
using WaybillService.Core.Waybills.Content;

namespace WaybillService.Application.Waybills.Paper.ContentFilling
{
    public interface IWaybillFileParser
    {
        Task<WaybillContent> Parse(Stream waybillFileStream);
    }
}