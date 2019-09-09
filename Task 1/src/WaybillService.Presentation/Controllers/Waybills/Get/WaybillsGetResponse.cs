using System;
using System.Collections.Generic;

namespace WaybillService.Presentation.Controllers.Waybills.Get
{
    public class WaybillsGetResponse
    {
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string Consignee { get; set; }

        public IReadOnlyList<WaybillsGetItemResponse> Items { get; set; }

        public decimal? TotalSumWithoutVat { get; set; }
        public decimal? TotalSumWithVat { get; set; }
        public decimal? TotalVatSum { get; set; }
        
        public WaybillsGetValidationResultResponse ValidationResult { get; set; }
    }
}