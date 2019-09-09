using System;

namespace WaybillService.Database.Context.Waybills.Models
{
    public class WaybillFileDto
    {
        public Guid CephId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Exact time (tick precision).
        /// Safe to use for direct equality comparisons.
        /// </summary>
        public DateTimeOffset UploadTime { get; set; }

        /// <summary>
        /// Only for displaying purposes while browsing DB.
        /// Not for equality comparisons.
        /// </summary>
        public DateTimeOffset DbDisplayedUploadTime
        {
            // Workaround for PgSQL capable to handle dates only with 1us precision,
            // whereas .NET can handle 100ns precision.
            get => UploadTime;
            set { } // required by EF Core
        }
    }
}