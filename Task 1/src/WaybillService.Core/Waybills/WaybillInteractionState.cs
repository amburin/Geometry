namespace WaybillService.Core.Waybills
{
    /// <summary>Represents state of waybill interaction with 3rd-party systems.</summary>
    public enum WaybillInteractionState
    {
        // Use this members to determine Waybill's overall state
        // in case of no data from Metazon is stored in Waybill model.
        // If adding some Metazon interaction related data to Waybill model,
        // eliminate corresponding enum member and use obtained data to determine Waybill state instead.
        None = 0,
        SentToMetazon,
        MetazonValidationReceived,
        CancellationRequestSent,
        CancellationResponseReceived,
    }
}