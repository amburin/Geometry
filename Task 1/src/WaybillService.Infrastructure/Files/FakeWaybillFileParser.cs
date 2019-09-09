using System;
using System.IO;
using System.Threading.Tasks;
using WaybillService.Application.Waybills.Paper.ContentFilling;
using WaybillService.Core.Waybills.Content;
using WaybillService.Core.Waybills.Content.Header;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Core.Waybills.Content.Totals;

namespace WaybillService.Infrastructure.Files
{
    /// <summary>
    /// Emulates call of parsing service
    /// </summary>
    public class FakeWaybillFileParser : IWaybillFileParser
    {
        public Task<WaybillContent> Parse(Stream waybillFileStream)
        {
            var header = new WaybillHeader(
                documentDate: new DateTime(2019, 06, 28),
                documentNumber: "1",
                consignee: "ООО ВЕКТОР"
            );

            var totals = new WaybillTotals(
                totalSumWithoutVat: 80508.47m,
                totalVatSum: 14491.53m,
                totalSumWithVat: 95000m
            );

            var content = new WaybillContent(
                header,
                new[] {CreateItem()},
                totals
            );

            return Task.FromResult(content);
        }

        private WaybillItem CreateItem()
        {
            var unit = new Unit(
                name: "шт",
                okeiCode: "шт");

            var monetaryValue = new MonetaryValue(
                amount: 1,
                priceWithoutVat: 80508.47m,
                vatPercent: 18m,
                sumWithoutVat: 80508.47m,
                vatSum: 14491.53m,
                sumWithVat: 95000m
            );

            var customsInfo = new WaybillCustomsInfo("RU", "320-ООУ", "71");

            return new WaybillItem(
                null,
                "Разработка проектно-сметной документации",
                "RVR-1",
                unit,
                monetaryValue,
                customsInfo
            );
        }
    }
}