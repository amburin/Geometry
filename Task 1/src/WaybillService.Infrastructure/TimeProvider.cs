using System;
using WaybillService.Core;

namespace WaybillService.Infrastructure
{
    public class TimeProvider : ITimeProvider
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}