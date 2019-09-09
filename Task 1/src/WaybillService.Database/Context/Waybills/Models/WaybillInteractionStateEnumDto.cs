namespace WaybillService.Database.Context.Waybills.Models
{
    public enum WaybillInteractionStateEnumDto
    {
        None = 0,
        SentToMetazon = 1,
        MetazonValidationReceived = 2,
        CancellationRequestSent = 3,
        CancellationResponseReceived = 4,
    }
}