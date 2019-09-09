using System.Threading.Tasks;
using Hangfire;
using WaybillService.Application.Waybills.Paper.StructuralValidation;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Events;
using WaybillService.Infrastructure.EventPublisher;

namespace WaybillService.Infrastructure.Waybills.Handlers
{
    public class ValidateWaybill : IHandler<WaybillParsed>
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IWaybillStructuralValidationFiller _waybillStructuralValidationFiller;

        public ValidateWaybill(
            IBackgroundJobClient backgroundJobClient,
            IWaybillStructuralValidationFiller waybillStructuralValidationFiller)
        {
            _backgroundJobClient = backgroundJobClient;
            _waybillStructuralValidationFiller = waybillStructuralValidationFiller;
        }

        public void Handle(WaybillParsed businessEvent)
        {
            _backgroundJobClient.Enqueue<ValidateWaybill>(
                x => x.HandleInternal(businessEvent.WaybillId));
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public Task HandleInternal(WaybillId waybillId)
        {
            return _waybillStructuralValidationFiller.FillStructuralValidation(waybillId);
        }
    }
}