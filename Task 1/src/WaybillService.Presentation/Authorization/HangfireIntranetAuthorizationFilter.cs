using Hangfire.Dashboard;

namespace WaybillService.Presentation.Authorization
{
    public class HangfireIntranetAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // Allow full access to everyone because service will be hosted
            // in docker container and will be exposed to intranet only.
            return true;
        }
    }
}