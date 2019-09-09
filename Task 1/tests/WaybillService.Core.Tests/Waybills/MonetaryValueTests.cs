using FluentAssertions;
using WaybillService.Core.Waybills.Content.Item;
using Xunit;

namespace WaybillService.Core.Tests.Waybills
{
    public class MonetaryValueTests
    {
        private int someAmount = 1001;
        private decimal somePrice = 10012;
        
        [Fact]
        public void If_sums_are_valid_result_shows_no_errors()
        {
            var monetaryValue = new MonetaryValue(
                amount: 1,
                priceWithoutVat: 10,
                vatPercent: 10,
                sumWithoutVat: 10,
                vatSum: 1,
                sumWithVat: 11
            );

            var result = monetaryValue.Validate();
            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void If_sum_with_vat_not_matches_sum_without_vat_result_shows_error()
        {
            var vatPercent = 10m;
            var sumWithoutVat = 10m;
            var sumWithVat = 11 + 1m;
            
            var monetaryValue = new MonetaryValue(
                amount: someAmount,
                priceWithoutVat: somePrice,
                vatPercent: vatPercent,
                sumWithoutVat: sumWithoutVat,
                vatSum: 10m,
                sumWithVat: sumWithVat
            );

            var result = monetaryValue.Validate();

            result.HasErrors.Should().BeTrue();
            result.HasInvalidSumWithVat.Should().BeTrue();
        }
    }
}