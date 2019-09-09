using System.Collections.Generic;
using System.Threading.Tasks;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Events;
using WaybillService.Core.Waybills.Paper;

namespace WaybillService.Application.Waybills.Paper.FileImport
{
    public class WaybillImporter : IWaybillImporter
    {
        private readonly IWaybillFileRepository _waybillFileRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPaperWaybillRepository _paperWaybillRepository;
        private readonly ITimeProvider _timeProvider;

        public WaybillImporter(
            IWaybillFileRepository waybillFileRepository,
            IEventPublisher eventPublisher,
            ITimeProvider timeProvider,
            IPaperWaybillRepository paperWaybillRepository)
        {
            _waybillFileRepository = waybillFileRepository;
            _eventPublisher = eventPublisher;
            _timeProvider = timeProvider;
            _paperWaybillRepository = paperWaybillRepository;
        }

        public async Task ImportFromFiles(
            OrderId orderId,
            IReadOnlyCollection<File> files)
        {
            var uploadDate = _timeProvider.Now();

            var waybillIds = new List<WaybillId>();
            foreach (var file in files)
            {
                var waybillFile = new WaybillFile(file, uploadDate);
                var fileId = await _waybillFileRepository.Save(waybillFile);

                var waybill = new PaperWaybill(orderId, fileId);
                var waybillId = await _paperWaybillRepository.Add(waybill);

                waybillIds.Add(waybillId);
            }

            //TODO [SSS-497] use Hangfire-PRO to allow batch job operations
            foreach (var waybillId in waybillIds)
            {
                _eventPublisher.Raise(new PaperWaybillImported(waybillId));
            }
        }
    }
}