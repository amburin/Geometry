using System;

namespace WaybillService.Database.Context
{
    public abstract class EnumValuesDto<TEnumDto> where TEnumDto : Enum
    {
        public TEnumDto Id { get; set; }
        public string Name { get; set; }
    }
}