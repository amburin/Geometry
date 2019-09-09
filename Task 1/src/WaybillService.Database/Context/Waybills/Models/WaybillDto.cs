using System;
using WaybillService.Database.Context.Waybills.Models.ValidationResult;

namespace WaybillService.Database.Context.Waybills.Models
{
    public class WaybillDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public Guid? SourceFileId { get; set; }
        public WaybillFileDto File { get; set; }
        public WaybillContentDto Content { get; set; }
        public WaybillValidationResultDto ValidationResult { get; set; }
        public WaybillInteractionStateEnumDto InteractionState { get; set; }
    }
}