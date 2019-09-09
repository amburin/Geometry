using System;
using System.Linq;
using WaybillService.Core;
using WaybillService.Core.Files;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Content;
using WaybillService.Core.Waybills.Content.Header;
using WaybillService.Core.Waybills.Content.Totals;
using WaybillService.Core.Waybills.Paper;
using WaybillService.Database.Context.Waybills.Models;
using WaybillService.Infrastructure.Waybills.Repositories;

namespace WaybillService.Infrastructure.Waybills
{
    internal static class WaybillMapper
    {
        public static void FillFromWaybillModel(this WaybillDto dto, Waybill model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            dto.Id = model.Id.Value;
            dto.OrderId = model.OrderId.Value;
            
            dto.FillContentFromModel(model.Content, model.Id);
        }

        public static void FillFromPaperWaybillModel(this WaybillDto dto, PaperWaybill model)
        {
            FillFromWaybillModel(dto, model);
            dto.SourceFileId = model.SourceFileId.ToGuid();
            dto.FillValidationResult(model.StructuralValidationResult, model.Id);
            dto.InteractionState = model.InteractionState.MapToDto();
        }

        
        public static PaperWaybill MapToPaperWaybill(this WaybillDto waybillDto)
        {
            if (waybillDto == null)
            {
                throw new ArgumentNullException(nameof(waybillDto));
            }

            var id = new WaybillId(waybillDto.Id);
            var fileId = new CephFileId(waybillDto.File.CephId);
            var orderId = new OrderId(waybillDto.OrderId);
            
            var waybillContent = waybillDto.Content?.MapToModel();
            var structuralValidationResult = waybillDto.ValidationResult?.MapToModel();

            var interactionState = waybillDto.InteractionState.MapToModel();

            return new PaperWaybill(
                id: id,
                sourceFileId: fileId,
                orderId: orderId,
                content: waybillContent,
                structuralValidationResult: structuralValidationResult,
                interactionState: interactionState);
        }

        private static void FillContentFromModel(
            this WaybillDto parentDto,
            WaybillContent model,
            WaybillId waybillId)
        {
            if (model == null)
            {
                parentDto.Content = null;
                return;
            }

            if (parentDto.Content == null)
            {
                parentDto.Content = new WaybillContentDto();
            }

            var dto = parentDto.Content;

            dto.SupplierWaybillId = waybillId.Value;

            dto.DocumentNumber = model.Header.DocumentNumber;
            dto.DocumentDate = model.Header.DocumentDate;
            dto.Consignee = model.Header.Consignee;

            dto.Items = model.Items
                .Select((item, i) => item.MapToDto(waybillId: waybillId, sequenceNumber: i + 1))
                .ToList();

            dto.TotalVat = model.Totals.TotalVatSum;
            dto.TotalSumWithVat = model.Totals.TotalSumWithVat;
            dto.TotalSumWithoutVat = model.Totals.TotalSumWithoutVat;
        }

        private static WaybillContent MapToModel(this WaybillContentDto dto)
        {
            var documentHeader = new WaybillHeader(
                documentNumber: dto.DocumentNumber,
                documentDate: dto.DocumentDate,
                consignee: dto.Consignee);

            var documentItems = dto.Items
                .Select(WaybillItemMapper.MapToModel)
                .ToArray();

            var documentTotals = new WaybillTotals(
                totalSumWithoutVat: dto.TotalSumWithVat,
                totalVatSum: dto.TotalVat,
                totalSumWithVat: dto.TotalSumWithoutVat);

            return new WaybillContent(
                header: documentHeader,
                items: documentItems,
                totals: documentTotals);
        }

        private static WaybillInteractionStateEnumDto MapToDto(
            this WaybillInteractionState model)
        {
            switch (model)
            {
                case WaybillInteractionState.None:
                    return WaybillInteractionStateEnumDto.None;
                case WaybillInteractionState.SentToMetazon:
                    return WaybillInteractionStateEnumDto.SentToMetazon;
                case WaybillInteractionState.MetazonValidationReceived:
                    return WaybillInteractionStateEnumDto.MetazonValidationReceived;
                case WaybillInteractionState.CancellationRequestSent:
                    return WaybillInteractionStateEnumDto.CancellationRequestSent;
                case WaybillInteractionState.CancellationResponseReceived:
                    return WaybillInteractionStateEnumDto.CancellationResponseReceived;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(model),
                        model,
                        $"There are no corresponding {nameof(WaybillInteractionStateEnumDto)} " +
                        $"for {nameof(WaybillInteractionState)}.{model.ToString()}.");
            }
        }

        private static WaybillInteractionState MapToModel(
            this WaybillInteractionStateEnumDto dto)
        {
            switch (dto)
            {
                case WaybillInteractionStateEnumDto.None:
                    return WaybillInteractionState.None;
                case WaybillInteractionStateEnumDto.SentToMetazon:
                    return WaybillInteractionState.SentToMetazon;
                case WaybillInteractionStateEnumDto.MetazonValidationReceived:
                    return WaybillInteractionState.MetazonValidationReceived;
                case WaybillInteractionStateEnumDto.CancellationRequestSent:
                    return WaybillInteractionState.CancellationRequestSent;
                case WaybillInteractionStateEnumDto.CancellationResponseReceived:
                    return WaybillInteractionState.CancellationResponseReceived;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(dto),
                        dto,
                        $"There are no corresponding {nameof(WaybillInteractionState)} " +
                        $"for {nameof(WaybillInteractionStateEnumDto)}.{dto.ToString()}.");
            }
        }
    }
}