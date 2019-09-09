using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Database.Context.Waybills.Models;

namespace WaybillService.Infrastructure.Waybills
{
    public static class WaybillItemMapper
    {
         public static WaybillItemDto MapToDto(
            this WaybillItem model,
            WaybillId waybillId,
            int sequenceNumber)
        {
            return new WaybillItemDto
            {
                SequenceNumber = sequenceNumber,
                SupplierWaybillId = waybillId.Value,
                RezonItemId = model.ItemId?.Value,

                Code = model.Code,
                Name = model.Name,
                Amount = model.MonetaryValue.Amount,
                Unit = model.Unit?.Name,
                UnitOkeiCode = model.Unit?.OkeiCode,

                PriceWithoutVat = model.MonetaryValue.PriceWithoutVat,
                SumWithoutVat = model.MonetaryValue.SumWithoutVat,
                SumWithVat = model.MonetaryValue.SumWithVat,
                VatPercent = model.MonetaryValue.VatPercent,
                VatSum = model.MonetaryValue.VatSum,

                CountryOfOriginCode = model.CustomsInfo?.CountryOfOriginCode,
                CountryOfOriginName = model.CustomsInfo?.CountryOfOriginName,
                CustomsDeclarationNumber = model.CustomsInfo?.CustomsDeclarationNumber,
            };
        }

        public static WaybillItem MapToModel(this WaybillItemDto waybillItemDto)
        {
            var itemId = waybillItemDto.RezonItemId.HasValue
                ? new ItemId(waybillItemDto.RezonItemId.Value)
                : null;

            var unit = new Unit(
                name: waybillItemDto.Unit,
                okeiCode: waybillItemDto.UnitOkeiCode);

            var itemTotals = new MonetaryValue(
                amount: waybillItemDto.Amount,
                priceWithoutVat: waybillItemDto.PriceWithoutVat,
                vatPercent: waybillItemDto.VatPercent,
                sumWithoutVat: waybillItemDto.SumWithoutVat,
                vatSum: waybillItemDto.VatSum,
                sumWithVat: waybillItemDto.SumWithVat
            );

            var customsInfo = new WaybillCustomsInfo(
                countryOfOriginName: waybillItemDto.CountryOfOriginName,
                customsDeclarationNumber: waybillItemDto.CustomsDeclarationNumber,
                countryOfOriginCode: waybillItemDto.CountryOfOriginCode
            );

            return new WaybillItem(
                itemId: itemId,
                name: waybillItemDto.Name,
                code: waybillItemDto.Code,
                unit: unit,
                monetaryValue: itemTotals,
                customsInfo: customsInfo
            );
        }
    }
}