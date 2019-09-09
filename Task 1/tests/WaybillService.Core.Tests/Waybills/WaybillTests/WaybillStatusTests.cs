using System;
using FluentAssertions;
using WaybillService.Core.Files;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Content;
using WaybillService.Core.Waybills.Content.Header;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Core.Waybills.Content.Totals;
using WaybillService.Core.Waybills.Paper;
using Xunit;

namespace WaybillService.Core.Tests.Waybills.WaybillTests
{
    public class WaybillStatusTests
    {
        private readonly OrderId _someOrderId = new OrderId(123L);
        private readonly CephFileId _someCephFileId = new CephFileId(new Guid("12345678-abcd-abcd-abcd-12345678abcd"));
        private readonly WaybillId _someWaybillId = new WaybillId(123L);

        private readonly WaybillContent _someContent = new WaybillContent(
            new WaybillHeader(default, default, default),
            new WaybillItem[0],
            new WaybillTotals(default, default, default));
        
        
        private readonly WaybillContentStructuralValidationResult _someStructuralValidationResult =
            new WaybillContentStructuralValidationResult(
                new WaybillHeaderValidationResult(default, default, default),
                new WaybillItemValidationResult[0],
                new WaybillTotalsValidationResult(default, default, default, default, default, default));

        [Fact]
        public void When_just_created_status_is_new()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.New);
        }

        [Fact]
        public void When_id_is_set_status_is_imported()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);
            waybill.SetId(_someWaybillId);

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.Imported);
        }

        [Fact]
        public void If_type_is_set_status_is_imported()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);
            waybill.SetId(_someWaybillId);

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.Imported);
        }

        [Fact]
        public void If_content_is_set_status_is_parsed()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);
            waybill.SetId(_someWaybillId);
            waybill.SetContent(_someContent);

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.Parsed);
        }

        [Fact]
        public void If_structural_validation_is_set_status_is_validated()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);
            waybill.SetId(_someWaybillId);
            waybill.SetContent(_someContent);
            waybill.SetStructuralValidationResult(_someStructuralValidationResult);

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.StructurallyValidated);
        }

        [Fact]
        public void If_waybill_is_sent_to_wms_status_is_structurally_validated()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);
            waybill.SetId(_someWaybillId);
            waybill.SetContent(_someContent);
            waybill.SetStructuralValidationResult(_someStructuralValidationResult);
            waybill.MarkAsSentToWms();

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.StructurallyValidated);
        }
        

        [Fact]
        public void If_wms_validation_received_status_is_sent_to_warehouse()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);
            waybill.SetId(_someWaybillId);
            waybill.SetContent(_someContent);
            waybill.SetStructuralValidationResult(_someStructuralValidationResult);
            waybill.MarkAsSentToWms();
            waybill.MarkAsValidatedInWms();

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.SentToWarehouse);
        }

        [Fact]
        public void If_waybill_has_benn_cancelled_status_is_cancelled()
        {
            // Arrange
            var waybill = new PaperWaybill(_someOrderId, _someCephFileId);
            waybill.SetId(_someWaybillId);
            waybill.SetContent(_someContent);
            waybill.SetStructuralValidationResult(_someStructuralValidationResult);
            waybill.MarkAsSentToWms();
            waybill.MarkAsValidatedInWms();
            waybill.MarkAsScheduledForCancellation();
            waybill.ApproveCancellation();

            // Act
            var state = waybill.Status;

            // Assert
            state.Should().Be(WaybillStatus.Cancelled);
        }
    }
}
