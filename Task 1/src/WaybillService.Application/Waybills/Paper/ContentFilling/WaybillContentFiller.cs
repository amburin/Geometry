using System.Threading.Tasks;
using WaybillService.Application.Waybills.Paper.FileImport;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Events;
using WaybillService.Core.Waybills.Paper;

namespace WaybillService.Application.Waybills.Paper.ContentFilling
{
    public class WaybillContentFiller : IWaybillContentFiller
    {
        private readonly IPaperWaybillRepository _waybillRepository;
        private readonly IWaybillFileRepository _waybillFileRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWaybillFileParser _parser;

        public WaybillContentFiller(
            IPaperWaybillRepository waybillRepository,
            IWaybillFileRepository waybillFileRepository,
            IEventPublisher eventPublisher,
            IWaybillFileParser parser)
        {
            _waybillRepository = waybillRepository;
            _waybillFileRepository = waybillFileRepository;
            _eventPublisher = eventPublisher;
            _parser = parser;
        }

        public async Task FillContent(WaybillId waybillId)
        {
            var waybill = await _waybillRepository.Get(waybillId);
            var file = await _waybillFileRepository.Get(waybill.SourceFileId);

            await FillContent(waybill, file);
            await _waybillRepository.Update(waybill);

            _eventPublisher.Raise(new WaybillParsed(waybillId));
        }

        private async Task FillContent(PaperWaybill waybill, WaybillFile file)
        {
            var waybillContent = await _parser.Parse(file.CreateFileStream());
            waybill.SetContent(waybillContent);
        }
    }
}