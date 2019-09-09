using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Paper;
using WaybillService.Database.Context;
using WaybillService.Database.Context.Waybills.Models;
using WaybillService.Database.Context.Waybills.Models.ValidationResult;

namespace WaybillService.Infrastructure.Waybills
{
    public sealed class WaybillRepository :
        IPaperWaybillRepository,
        IWaybillRepository
    {
        private readonly WaybillContext _context;

        public WaybillRepository(WaybillContext context)
        {
            _context = context;
        }

        async Task<Waybill> IWaybillRepository.Get(WaybillId waybillId)
        {
            var dto = await QueryById(waybillId)
                .SingleOrDefaultAsync();

            return dto.MapToPaperWaybill();
        }

        async Task<PaperWaybill> IPaperWaybillRepository.Get(WaybillId waybillId)
        {
            var dto = await QueryById(waybillId)
                .Where(x => x.SourceFileId != null)
                .SingleOrDefaultAsync();

            return dto.MapToPaperWaybill();
        }

        public async Task<WaybillId> Add(PaperWaybill waybill)
        {
            var dto = await CreateEmptyWaybillDto(waybill);
            dto.FillFromPaperWaybillModel(waybill);
            await _context.SaveChangesAsync();
            return waybill.Id;
        }

        public async Task Update(PaperWaybill waybill)
        {
            var dto = await QueryById(waybill.Id)
                .SingleOrDefaultAsync();

            RemoveChildren(dto);
            await _context.SaveChangesAsync();

            dto.FillFromPaperWaybillModel(waybill);
            _context.Waybills.Update(dto);
            await _context.SaveChangesAsync();
        }

        private void RemoveChildren(WaybillDto persistedDto)
        {
            if (persistedDto.Content?.Items != null)
            {
                _context.Set<WaybillItemDto>().RemoveRange(persistedDto.Content.Items);
            }

            if (persistedDto.ValidationResult?.ItemValidationResults != null)
            {
                _context.Set<WaybillItemValidationResultDto>()
                    .RemoveRange(persistedDto.ValidationResult.ItemValidationResults);
            }
        }

        private IQueryable<WaybillDto> QueryById(WaybillId waybillId)
        {
            return _context.Waybills
                .Include(x => x.File)
                .Include(x => x.Content)
                .ThenInclude(x => x.Items)
                .Where(x => x.Id == waybillId.Value);
        }

        private async Task<WaybillDto> CreateEmptyWaybillDto(Waybill waybill)
        {
            var dto = new WaybillDto();
            await _context.Waybills.AddAsync(dto);
            await _context.SaveChangesAsync();
            var id = new WaybillId(dto.Id);
            //We are setting id for further mapping
            waybill.SetId(id);
            return dto;
        }
    }
}