using System.Linq;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Content;
using WaybillService.Core.Waybills.Content.Header;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Core.Waybills.Content.Totals;
using WaybillService.Database.Context.Waybills.Models;
using WaybillService.Database.Context.Waybills.Models.ValidationResult;

namespace WaybillService.Infrastructure.Waybills.Repositories
{
    public static class WaybillValidationResultMapper
    {
        public static void FillValidationResult(this WaybillDto parentDto,
            WaybillContentStructuralValidationResult model, WaybillId waybillId)
        {
            if (model == null)
            {
                parentDto.ValidationResult = null;
                return;
            }

            if (parentDto.ValidationResult == null)
            {
                parentDto.ValidationResult = new WaybillValidationResultDto();
            }

            var dto = parentDto.ValidationResult;

            dto.HasConsigneeError = model.HeaderResult.HasConsigneeError;
            dto.HasDocumentDateError = model.HeaderResult.HasDocumentDateError;
            dto.HasDocumentNumberError = model.HeaderResult.HasDocumentNumberError;
            dto.HasEmptySumWithoutVat = model.TotalsResult.HasEmptySumWithoutVat;
            dto.HasEmptyTotalVatSum = model.TotalsResult.HasEmptyTotalVatSum;
            dto.HasEmptySumWithVat = model.TotalsResult.HasEmptySumWithVat;
            dto.HasInvalidSumWithoutVat = model.TotalsResult.HasInvalidSumWithoutVat;
            dto.HasInvalidTotalVatSum = model.TotalsResult.HasInvalidTotalVatSum;
            dto.HasInvalidSumWithVat = model.TotalsResult.HasInvalidSumWithVat;

            dto.ItemValidationResults = model.ItemResults
                .Select((itemDto, i) => itemDto.MapToDto(waybillId, i + 1))
                .ToList();
        }

        public static WaybillContentStructuralValidationResult MapToModel(
            this WaybillValidationResultDto waybillDto)
        {
            var headerResult = new WaybillHeaderValidationResult(
                hasConsigneeError: waybillDto.HasConsigneeError,
                hasDocumentDateError: waybillDto.HasDocumentDateError,
                hasDocumentNumberError: waybillDto.HasDocumentNumberError);

            var totalsResult = new WaybillTotalsValidationResult(
                hasEmptySumWithoutVat: waybillDto.HasEmptySumWithoutVat,
                hasEmptySumWithVat: waybillDto.HasEmptySumWithVat,
                hasEmptyTotalVatSum: waybillDto.HasEmptyTotalVatSum,
                hasInvalidTotalVatSum: waybillDto.HasInvalidTotalVatSum,
                hasInvalidSumWithVat: waybillDto.HasInvalidSumWithVat,
                hasInvalidSumWithoutVat: waybillDto.HasInvalidSumWithoutVat);

            var items = waybillDto.ItemValidationResults
                .Select(MapToModel)
                .ToArray();

            return new WaybillContentStructuralValidationResult(
                headerResult: headerResult,
                itemResults: items,
                totalsResult: totalsResult);
        }

        private static WaybillItemValidationResultDto MapToDto(
            this WaybillItemValidationResult model,
            WaybillId waybillId,
            int sequenceNumber)
        {
            return new WaybillItemValidationResultDto
            {
                WaybillId = waybillId.Value,
                WaybillItemSequenceNumber = sequenceNumber,
                HasEmptyItemId = model.HasEmptyItemId,
                HasEmptyCode = model.HasEmptyCode,
                HasEmptyName = model.HasEmptyName,
                HasEmptyOkeiUnitCode = model.HasEmptyOkeiUnitCode,
                HasEmptyUnitName = model.HasEmptyUnitName,

                HasEmptyAmount = model.MonetaryValueValidationResult.HasEmptyAmount,
                HasEmptyPriceWithoutVat = model.MonetaryValueValidationResult
                    .HasEmptyPriceWithoutVat,
                HasInvalidVatPercent = model.MonetaryValueValidationResult.HasInvalidVatPercent,
                HasEmptySumWithVat = model.MonetaryValueValidationResult.HasEmptySumWithVat,
                HasEmptySumWithoutVat = model.MonetaryValueValidationResult.HasEmptySumWithoutVat,
                HasInvalidVatSum = model.MonetaryValueValidationResult.HasInvalidVatSum,
                HasInvalidSumWithVat = model.MonetaryValueValidationResult.HasInvalidSumWithVat,

                HasEmptyCustomsDeclarationNumber = model
                    .CustomsInfoValidationResult.HasEmptyCustomDeclarationNumber,
                HasEmptyCountryOfOriginCode = model
                    .CustomsInfoValidationResult.HasEmptyCountryOfOriginCode,
                HasEmptyCountryOfOriginName = model
                    .CustomsInfoValidationResult.HasEmptyCountryOfOriginName
            };
        }

        private static WaybillItemValidationResult MapToModel(
            this WaybillItemValidationResultDto itemDto)
        {
            var monetaryValueValidationResult = new MonetaryValueValidationResult(
                hasEmptyAmount: itemDto.HasEmptyAmount,
                hasEmptyPriceWithoutVat: itemDto.HasEmptyPriceWithoutVat,
                hasInvalidVatPercent: itemDto.HasInvalidVatPercent,
                hasInvalidVatSum: itemDto.HasInvalidVatSum,
                hasEmptySumWithVat: itemDto.HasEmptySumWithVat,
                hasEmptySumWithoutVat: itemDto.HasEmptySumWithoutVat,
                hasInvalidSumWithVat: itemDto.HasInvalidSumWithVat
            );

            var customsInfoValidationResult = new CustomsInfoValidationResult(
                hasEmptyCustomDeclarationNumber: itemDto.HasEmptyCustomsDeclarationNumber,
                hasEmptyCountryOfOriginCode: itemDto.HasEmptyCountryOfOriginCode,
                hasEmptyCountryOfOriginName: itemDto.HasEmptyCountryOfOriginName
            );

            return new WaybillItemValidationResult(
                hasEmptyItemId: itemDto.HasEmptyItemId,
                hasEmptyCode: itemDto.HasEmptyCode,
                hasEmptyName: itemDto.HasEmptyName,
                hasEmptyOkeiUnitCode: itemDto.HasEmptyOkeiUnitCode,
                hasEmptyUnitName: itemDto.HasEmptyUnitName,
                monetaryValueValidationResult,
                customsInfoValidationResult
            );
        }
    }
}