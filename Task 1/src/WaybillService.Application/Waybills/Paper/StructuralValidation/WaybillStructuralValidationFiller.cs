using System.Threading.Tasks;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Events;
using WaybillService.Core.Waybills.Paper;

namespace WaybillService.Application.Waybills.Paper.StructuralValidation
{
    public class WaybillStructuralValidationFiller : IWaybillStructuralValidationFiller
    {
        private readonly IPaperWaybillRepository _paperWaybillRepository;
        private readonly IEventPublisher _eventPublisher;

        public WaybillStructuralValidationFiller(
            IEventPublisher eventPublisher,
            IPaperWaybillRepository paperWaybillRepository)
        {
            _eventPublisher = eventPublisher;
            _paperWaybillRepository = paperWaybillRepository;
        }

        public async Task FillStructuralValidation(WaybillId waybillId)
        {
            var waybill = await _paperWaybillRepository.Get(waybillId);

            await FillValidationResult(waybill);
            await _paperWaybillRepository.Update(waybill);

            if (!waybill.HasValidationErrors)
            {
                _eventPublisher.Raise(new StructuralValidationPassed(waybill.Id));
            }
        }

        private async Task FillValidationResult(PaperWaybill waybill)
        {
            var validationResult = waybill.Content.Validate();
            waybill.SetStructuralValidationResult(validationResult);
        }
    }
}