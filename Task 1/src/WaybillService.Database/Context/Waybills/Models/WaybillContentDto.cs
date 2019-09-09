using System;
using System.Collections.Generic;

namespace WaybillService.Database.Context.Waybills.Models
{
    public class WaybillContentDto
    {
        public long SupplierWaybillId { get; set; }

        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string Consignee { get; set; }

        public List<WaybillItemDto> Items { get; set; }

        public decimal? TotalVat { get; set; }
        public decimal? TotalSumWithVat { get; set; }
        public decimal? TotalSumWithoutVat { get; set; }
    }
}