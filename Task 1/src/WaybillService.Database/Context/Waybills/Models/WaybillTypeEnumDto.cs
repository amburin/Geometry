namespace WaybillService.Database.Context.Waybills.Models
{
    // NOTE: this type is used for DB seeding: only non-zero values are valid.
    public enum WaybillTypeEnumDto
    {
        Unknown = -1,

        Utd1 = 1,
        Utd2 = 2,
        ConsignmentNote = 3,
        Invoice = 4,
    }
}