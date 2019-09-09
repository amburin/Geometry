using System;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Content;

namespace WaybillService.Application.Waybills.Provider
{
    public class GetWaybillsResult
    {
        public GetWaybillsResult(
            Waybill waybill,
            string name,
            DateTimeOffset uploadTime,
            WaybillContentStructuralValidationResult validationResult)
        {
            Waybill = waybill;
            FileName = name;
            FileUploadTime = uploadTime;
            ValidationResult = validationResult;
        }

        public Waybill Waybill { get; }
        public string FileName { get; }
        public DateTimeOffset FileUploadTime { get; }
        public WaybillContentStructuralValidationResult ValidationResult { get; }
    }
}