using System;
using System.Runtime.CompilerServices;
using WaybillService.Core.Files;
using WaybillService.Core.Waybills.Content;

namespace WaybillService.Core.Waybills.Paper
{
    public class PaperWaybill : Waybill
    {
        public PaperWaybill(OrderId orderId, CephFileId sourceFileId) : base(orderId)
        {
            SourceFileId = sourceFileId;
        }

        public PaperWaybill(
            WaybillId id,
            CephFileId sourceFileId,
            OrderId orderId,
            WaybillContent content,
            WaybillContentStructuralValidationResult structuralValidationResult,
            WaybillInteractionState interactionState)
            : base(
                id,
                orderId,
                content)
        {
            SourceFileId = sourceFileId;
            StructuralValidationResult = structuralValidationResult;
            InteractionState = interactionState;
        }

        public override WaybillStatus Status => GetExternalStatus();
        public override bool IsEdo => false;
        public bool HasValidationErrors => StructuralValidationResult.HasErrors;
        public CephFileId SourceFileId { get; }

        public WaybillContentStructuralValidationResult
            StructuralValidationResult { get; protected set; }

        public bool CanStartDeletion =>
            ProcessingState == WaybillProcessingState.WmsValidationReceived;

        private WaybillProcessingState ProcessingState => GetInternalState();
        public WaybillInteractionState InteractionState { get; protected set; }

        public override void SetId(WaybillId id)
        {
            ThrowIfSetMemberWhileNotInState(WaybillProcessingState.New);
            Id = id;
        }

        public void SetContent(WaybillContent content)
        {
            ThrowIfSetMemberWhileNotInState(WaybillProcessingState.Imported);
            
            Content = content;
        }

        public void SetStructuralValidationResult(
            WaybillContentStructuralValidationResult structuralValidationResult)
        {
            ThrowIfSetMemberWhileNotInState(WaybillProcessingState.ContentParsed);

            StructuralValidationResult = structuralValidationResult;
        }

        public void MarkAsScheduledForCancellation()
        {
            ThrowIfSetMemberWhileNotInState(WaybillProcessingState.WmsValidationReceived);

            InteractionState = WaybillInteractionState.CancellationRequestSent;
        }

        public void ApproveCancellation()
        {
            ThrowIfSetMemberWhileNotInState(WaybillProcessingState.CancellationSent);

            InteractionState = WaybillInteractionState.CancellationResponseReceived;
        }

        public void MarkAsSentToWms()
        {
            ThrowIfSetMemberWhileNotInState(WaybillProcessingState.StructurallyValidated);

            InteractionState = WaybillInteractionState.SentToMetazon;
        }

        public void MarkAsValidatedInWms()
        {
            ThrowIfSetMemberWhileNotInState(WaybillProcessingState.SentToWms);

            InteractionState = WaybillInteractionState.MetazonValidationReceived;
        }

        private WaybillStatus GetExternalStatus()
        {
            switch (ProcessingState)
            {
                case WaybillProcessingState.New:
                    return WaybillStatus.New;
                case WaybillProcessingState.Imported:
                    return WaybillStatus.Imported;
                case WaybillProcessingState.ContentParsed:
                    return WaybillStatus.Parsed;
                case WaybillProcessingState.StructurallyValidated:
                case WaybillProcessingState.SentToWms:
                    return WaybillStatus.StructurallyValidated;
                case WaybillProcessingState.WmsValidationReceived:
                case WaybillProcessingState.CancellationSent:
                    return HasValidationErrors
                        ? WaybillStatus.ProcessedWithError
                        : WaybillStatus.SentToWarehouse;
                case WaybillProcessingState.CancellationApprovalReceived:
                    return WaybillStatus.Cancelled;
                default:
                    throw new NotSupportedException(
                        $"{nameof(ProcessingState)}.{ProcessingState:G} " +
                        $"can not be mapped to {nameof(WaybillStatus)}.");
            }
        }

        private WaybillProcessingState GetInternalState()
        {
            switch (InteractionState)
            {
                case WaybillInteractionState.CancellationResponseReceived:
                    return WaybillProcessingState.CancellationApprovalReceived;
                case WaybillInteractionState.CancellationRequestSent:
                    return WaybillProcessingState.CancellationSent;
                case WaybillInteractionState.MetazonValidationReceived:
                    return WaybillProcessingState.WmsValidationReceived;
                case WaybillInteractionState.SentToMetazon:
                    return WaybillProcessingState.SentToWms;
                case WaybillInteractionState.None:
                    break;
                default:
                    throw new NotSupportedException(
                        $"{nameof(InteractionState)}.{InteractionState:G} " +
                        $"can not be mapped to {nameof(WaybillProcessingState)}.");
            }

            if (StructuralValidationResult != default)
            {
                return WaybillProcessingState.StructurallyValidated;
            }

            if (Content != default)
            {
                return WaybillProcessingState.ContentParsed;
            }

            if (Id != default)
            {
                return WaybillProcessingState.Imported;
            }

            return WaybillProcessingState.New;
        }

        private void ThrowIfSetMemberWhileNotInState(
            WaybillProcessingState onlyAllowedState,
            [CallerMemberName] string memberName = null)
        {
            if (ProcessingState != onlyAllowedState)
            {
                throw new InvalidOperationException(
                    $"Executing {memberName} for {nameof(Waybill)} with {Id}" +
                    $"in state {ProcessingState} is forbidden. " +
                    $"The only allowed state for this operation is {onlyAllowedState}.");
            }
        }
    }
}