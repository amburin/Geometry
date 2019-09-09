using System.Threading.Tasks;
using Hangfire;
using WaybillService.Application.Waybills.Paper.ContentFilling;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Events;
using WaybillService.Infrastructure.EventPublisher;

namespace WaybillService.Infrastructure.Waybills.Handlers
{
    public class ParseWaybill : IHandler<PaperWaybillImported>
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IWaybillContentFiller _waybillContentFiller;

        public ParseWaybill(
            IBackgroundJobClient backgroundJobClient,
            IWaybillContentFiller waybillContentFiller)
        {
            _backgroundJobClient = backgroundJobClient;
            _waybillContentFiller = waybillContentFiller;
        }

        public void Handle(PaperWaybillImported businessEvent)
        {
            _backgroundJobClient.Enqueue<ParseWaybill>(
                x => x.HandleInternal(businessEvent.WaybillId));
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public Task HandleInternal(WaybillId waybillId)
        {
            return _waybillContentFiller.FillContent(waybillId);
        }
    }
}