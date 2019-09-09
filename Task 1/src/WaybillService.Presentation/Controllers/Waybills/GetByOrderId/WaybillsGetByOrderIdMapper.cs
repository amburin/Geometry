using System.Linq;
using WaybillService.Application.Waybills.Provider;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Content;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Presentation.Controllers.Waybills.GetByOrderId.ValidationResult;

namespace WaybillService.Presentation.Controllers.Waybills.GetByOrderId
{
    public static class WaybillsGetByOrderIdMapper
    {
        public static WaybillsGetByOrderIdResponse MapToResponse(GetWaybillsResult result)
        {
            return new WaybillsGetByOrderIdResponse
            {
                SupplierWaybillId = result.Waybill.Id.Value,

                Source = result.Waybill.IsEdo
                    ? WaybillsGetSourceResponse.Edo
                    : WaybillsGetSourceResponse.Paper,
                
                DocumentNumber = result.Waybill.Content?.Header.DocumentNumber,
                DocumentDate = result.Waybill.Content?.Header.DocumentDate,

                FileName = result.FileName,
                UploadTime = result.FileUploadTime,

                ValidationResultResponse =
                    result.ValidationResult?.MapToResponse(),

                Status = result.Waybill.Status.MapToResponse(),
            };
        }

        private static WaybillsGetValidationResultResponse MapToResponse(
            this WaybillContentStructuralValidationResult waybillValidationResult)
        {
            var items = waybillValidationResult.ItemResults
                .Select(MapToResponse)
                .ToArray();

            return new WaybillsGetValidationResultResponse
            {
                HasConsigneeError = waybillValidationResult.HeaderResult.HasConsigneeError,
                HasDocumentDateError = waybillValidationResult.HeaderResult.HasDocumentDateError,
                HasDocumentNumberError =
                    waybillValidationResult.HeaderResult.HasDocumentNumberError,

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

                ItemValidationResults = items,

                HasErrors = waybillValidationResult.HasErrors,
                ErrorCount = waybillValidationResult.ErrorCount,
            };
        }

        private static WaybillsGetItemValidationResultResponse MapToResponse(
            this WaybillItemValidationResult waybillItemValidationResult)
        {
            return new WaybillsGetItemValidationResultResponse
            {
                HasEmptyItemId = waybillItemValidationResult.HasEmptyItemId,
                HasEmptyCode = waybillItemValidationResult.HasEmptyCode,
                HasEmptyName = waybillItemValidationResult.HasEmptyName,
                HasEmptyOkeiUnitCode = waybillItemValidationResult.HasEmptyOkeiUnitCode,
                HasEmptyUnitName = waybillItemValidationResult.HasEmptyUnitName,

                HasEmptyAmount = waybillItemValidationResult.MonetaryValueValidationResult
                    .HasEmptyAmount,
                HasEmptyPriceWithoutVat = waybillItemValidationResult.MonetaryValueValidationResult
                    .HasEmptyPriceWithoutVat,
                HasInvalidVatPercent = waybillItemValidationResult.MonetaryValueValidationResult
                    .HasInvalidVatPercent,
                HasEmptySumWithVat = waybillItemValidationResult.MonetaryValueValidationResult
                    .HasEmptySumWithVat,
                HasEmptySumWithoutVat = waybillItemValidationResult.MonetaryValueValidationResult
                    .HasEmptySumWithoutVat,
                HasInvalidVatSum = waybillItemValidationResult.MonetaryValueValidationResult
                    .HasInvalidVatSum,
                HasInvalidSumWithVat = waybillItemValidationResult.MonetaryValueValidationResult
                    .HasInvalidSumWithVat,

                HasEmptyCustomDeclarationNumber = waybillItemValidationResult
                    .CustomsInfoValidationResult.HasEmptyCustomDeclarationNumber,
                HasEmptyCountryOfOriginCode = waybillItemValidationResult
                    .CustomsInfoValidationResult.HasEmptyCountryOfOriginCode,
                HasEmptyCountryOfOriginName = waybillItemValidationResult
                    .CustomsInfoValidationResult.HasEmptyCountryOfOriginName,

                HasErrors = waybillItemValidationResult.HasErrors,
                ErrorCount = waybillItemValidationResult.ErrorCount,
            };
        }

        private static WaybillsGetStatusResponse MapToResponse(this WaybillStatus waybillStatus)
        {
            switch (waybillStatus)
            {
                case WaybillStatus.New:
                    return WaybillsGetStatusResponse.New;
                case WaybillStatus.Imported:
                    return WaybillsGetStatusResponse.Imported;
                case WaybillStatus.Parsed:
                    return WaybillsGetStatusResponse.Parsed;
                case WaybillStatus.StructurallyValidated:
                    return WaybillsGetStatusResponse.StructurallyValidated;
                case WaybillStatus.SentToWarehouse:
                    return WaybillsGetStatusResponse.SentToWarehouse;
                case WaybillStatus.Cancelled:
                    return WaybillsGetStatusResponse.Cancelled;
                case WaybillStatus.ProcessedWithError:
                    return WaybillsGetStatusResponse.ProcessedWithError;
                default:
                    throw new EnumNotMappedToResponseException(waybillStatus);
            }
        }
    }
}