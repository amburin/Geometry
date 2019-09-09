using System;
using Microsoft.AspNetCore.Mvc;

namespace WaybillService.Presentation.Infrastructure
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromCompanyIdHeaderAttribute : FromHeaderAttribute
    {
        public FromCompanyIdHeaderAttribute()
        {
            Name = "x-o3-company-id";
        }
    }
}
