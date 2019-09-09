using FluentAssertions;
using WaybillService.Core.Waybills.Content;
using WaybillService.Core.Waybills.Content.Header;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Core.Waybills.Content.Totals;
using Xunit;

namespace WaybillService.Core.Tests.Waybills
{
    public class WaybillDocumentValidationResultTests
    {
        [Fact]
        public void Errors_are_counted_correctly()
        {
            var document = new WaybillContentStructuralValidationResult(
                CreateHeaderResultWith1Error(),
                new[]
                {
                    CreateItemResultWith3Errors(),
                    CreateItemResultWith3Errors()
                },
                CreateTotalsResultWith1Error()
            );

            document.ErrorCount.Should().Be(8);
        }

        private static WaybillTotalsValidationResult CreateTotalsResultWith1Error()
        {
            return new WaybillTotalsValidationResult(true, false, false, false, false, false);
        }

        private static WaybillHeaderValidationResult CreateHeaderResultWith1Error()
        {
            return new WaybillHeaderValidationResult(true, false, false);
        }

        private static WaybillItemValidationResult CreateItemResultWith3Errors()
        {
            return new WaybillItemValidationResult(
                true,
                false,
                false,
                false,
                false,
                new MonetaryValueValidationResult(
                    true,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false),
                new CustomsInfoValidationResult(true, false, false)
            );
        }
    }
}