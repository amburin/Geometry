using System;

namespace WaybillService.Presentation
{
    public class EnumNotMappedToResponseException : NotSupportedException
    {
        public EnumNotMappedToResponseException(Enum @enum) : base(
            $"Cannot map value {@enum:G} to response")
        {
        }
    }
}