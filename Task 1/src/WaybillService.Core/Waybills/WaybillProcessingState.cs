namespace WaybillService.Core.Waybills
{
    /// <summary>
    /// Represents a <see cref="Waybill"/> lifecycle.
    /// Normally, states can only be transitioned in direct order.
    /// </summary>
    public enum WaybillProcessingState
    {
        New = 0,
        Imported,
        ContentParsed,
        StructurallyValidated,
        SentToWms,
        WmsValidationReceived,
        CancellationSent,
        CancellationApprovalReceived,
    }
}