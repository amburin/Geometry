namespace WaybillService.Core.Waybills
{
    /// <summary>
    /// Not to be confused with <see cref="WaybillProcessingState"/>.
    /// See <see ref="https://confluence.ozon.ru/pages/viewpage.action?pageId=111976902"/> for details.
    /// </summary>
    public enum WaybillStatus
    {
        New = 0,
        Imported,
        Parsed,
        StructurallyValidated,
        /// <summary>Means final successful state after receiving validation from Metazon.</summary>
        SentToWarehouse,
        /// <summary>Means any stage of processing failed for some reason.</summary>
        ProcessedWithError,
        Cancelled,
    }
}