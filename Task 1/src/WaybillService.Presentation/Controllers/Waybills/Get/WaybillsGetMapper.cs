using System.Linq;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Content;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Core.Waybills.Paper;

namespace WaybillService.Presentation.Controllers.Waybills.Get
{
    public static class WaybillsGetMapper
    {
        public static WaybillsGetResponse MapToResponse(Waybill waybill)
        {
            var response = new WaybillsGetResponse
            {
                DocumentNumber = waybill.Content?.Header?.DocumentNumber,
                DocumentDate = waybill.Content?.Header?.DocumentDate,
                Consignee = waybill.Content?.Header?.Consignee,

                Items = waybill.Content?.Items
                    .Select(MapToResponse)
                    .ToArray(),

                TotalSumWithoutVat = waybill.Content?.Totals?.TotalSumWithoutVat,
                TotalSumWithVat = waybill.Content?.Totals?.TotalSumWithVat,
                TotalVatSum = waybill.Content?.Totals?.TotalVatSum,
            };

            if (waybill is PaperWaybill paperWaybill)
            {
                response.ValidationResult =
                    paperWaybill.StructuralValidationResult?.MapToResponse();
            }

            return response;
        }

        private static WaybillsGetItemResponse MapToResponse(this WaybillItem waybillItem)
        {
            return new WaybillsGetItemResponse
            {
                RezonItemId = waybillItem.ItemId?.Value,
                Name = waybillItem.Name,
                Code = waybillItem.Code,

                Amount = waybillItem.MonetaryValue.Amount,
                PriceWithoutVat = waybillItem.MonetaryValue.PriceWithoutVat,
                SumWithoutVat = waybillItem.MonetaryValue.SumWithoutVat,
                SumWithVat = waybillItem.MonetaryValue.SumWithVat,
                SumVat = waybillItem.MonetaryValue.VatSum
            };
        }

        private static WaybillsGetValidationResultResponse MapToResponse(
            this WaybillContentStructuralValidationResult waybillValidationResult)
        {
            return new WaybillsGetValidationResultResponse
            {
                HasConsigneeError = waybillValidationResult.HeaderResult.HasConsigneeError,
                HasDocumentDateError = waybillValidationResult.HeaderResult.HasDocumentDateError,
                HasDocumentNumberError =
                    waybillValidationResult.HeaderResult.HasDocumentNumberError,

                ItemValidationResults = waybillValidationResult.ItemResults
                    .Select(MapToResponse)
                    .ToArray(),

                HasEmptySumWithoutVat = waybillValidationResult.TotalsResult
                    .HasEmptySumWithoutVat,
                HasEmptyTotalVatSum =
                    waybillValidationResult.TotalsResult.HasEmptyTotalVatSum,
                HasEmptySumWithVat =
                    waybillValidationResult.TotalsResult.HasEmptySumWithVat,
                HasInvalidSumWithoutVat = waybillValidationResult.TotalsResult
                    .HasInvalidSumWithoutVat,
                HasInvalidTotalVatSum = waybillValidationResult.TotalsResult
                    .HasInvalidTotalVatSum,
                HasInvalidSumWithVat =
                    waybillValidationResult.TotalsResult.HasInvalidSumWithVat,
            };
        }

        private static WaybillsGetItemValidationResultResponse MapToResponse(
            this WaybillItemValidationResult validationResult)
        {
            return new WaybillsGetItemValidationResultResponse
            {
                HasEmptyItemId = validationResult.HasEmptyItemId,
                HasEmptyCode = validationResult.HasEmptyCode,
                HasEmptyName = validationResult.HasEmptyName,
                HasEmptyOkeiUnitCode = validationResult.HasEmptyOkeiUnitCode,
                HasEmptyUnitName = validationResult.HasEmptyUnitName,

                HasEmptyAmount = validationResult.MonetaryValueValidationResult.HasEmptyAmount,
                HasEmptyPriceWithoutVat = validationResult.MonetaryValueValidationResult
                    .HasEmptyPriceWithoutVat,
                HasInvalidVatPercent =
                    validationResult.MonetaryValueValidationResult.HasInvalidVatPercent,
                HasEmptySumWithVat =
                    validationResult.MonetaryValueValidationResult.HasEmptySumWithVat,
                HasEmptySumWithoutVat = validationResult.MonetaryValueValidationResult
                    .HasEmptySumWithoutVat,
                HasInvalidVatSum = validationResult.MonetaryValueValidationResult.HasInvalidVatSum,
                HasInvalidSumWithVat =
                    validationResult.MonetaryValueValidationResult.HasInvalidSumWithVat,

                HasEmptyCustomDeclarationNumber = validationResult.CustomsInfoValidationResult
                    .HasEmptyCustomDeclarationNumber,
                HasEmptyCountryOfOriginCode = validationResult.CustomsInfoValidationResult
                    .HasEmptyCountryOfOriginCode,
                HasEmptyCountryOfOriginName = validationResult.CustomsInfoValidationResult
                    .HasEmptyCountryOfOriginName,
            };
        }
    }
}