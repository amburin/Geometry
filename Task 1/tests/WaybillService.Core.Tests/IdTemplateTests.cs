using FluentAssertions;
using Newtonsoft.Json;
using WaybillService.Core.Waybills;
using Xunit;

namespace WaybillService.Core.Tests
{
    public class IdTemplateTests
    {
        [Fact]
        public void Serialization_works_both_ways()
        {
            // Arrange
            var typedId = new OrderId(123);

            // Act
            var serializedId = JsonConvert.SerializeObject(typedId);
            var deserializedId = JsonConvert.DeserializeObject<OrderId>(serializedId);

            // Assert
            deserializedId.Should().Be(typedId);
        }
    }
}
