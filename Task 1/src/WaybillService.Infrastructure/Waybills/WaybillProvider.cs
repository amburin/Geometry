using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaybillService.Application.Waybills.Provider;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Database.Context;
using WaybillService.Database.Context.Waybills.Models;
using WaybillService.Infrastructure.Waybills.Repositories;

namespace WaybillService.Infrastructure.Waybills
{
    public class WaybillProvider : IWaybillProvider
    {
        private readonly WaybillContext _context;

        public WaybillProvider(WaybillContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<GetWaybillsResult>> Get(OrderId orderId)
        {
            var documents = await _context.Waybills
                .Where(x => x.OrderId == orderId.Value)
                .Include(x => x.Content)
                .ThenInclude(x => x.Items)
                .Include(x => x.File)
                .Include(x => x.ValidationResult)
                .ThenInclude(x => x.ItemValidationResults)
                .OrderByDescending(x => x.File.UploadTime)
                .ToArrayAsync();

            return documents
                .Select(x => new {Waybill = x, x.File.UploadTime, x.File.Name})
                .Select(x => GetWaybillsResult(x.Waybill, x.Name, x.UploadTime))
                .ToArray();
        }

        private static GetWaybillsResult GetWaybillsResult(
            WaybillDto waybillDto,
            string fileName,
            DateTimeOffset uploadTime)
        {
            return new GetWaybillsResult(
                waybillDto.MapToPaperWaybill(),
                fileName,
                uploadTime,
                waybillDto.ValidationResult?.MapToModel()
            );
        }
    }
}