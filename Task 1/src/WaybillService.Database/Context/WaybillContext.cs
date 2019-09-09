using Microsoft.EntityFrameworkCore;
using WaybillService.Database.Context.Waybills;
using WaybillService.Database.Context.Waybills.Models;

namespace WaybillService.Database.Context
{
    public class WaybillContext : DbContext
    {
        /// <summary>Ctor normally used by <see cref="SupplierContextFactory" /> via dotnet EF CLI tool.</summary>
        public WaybillContext(DbContextOptions<WaybillContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WaybillFileDto> WaybillFiles { get; set; }
        public virtual DbSet<WaybillDto> Waybills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            WaybillsConfiguration.ConfigureWaybills(modelBuilder);
        }
    }
}