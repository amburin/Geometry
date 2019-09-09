using System;

namespace WaybillService.Core
{
    public interface ITimeProvider
    {
        DateTimeOffset Now();
    }
}