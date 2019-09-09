using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WaybillService.Application;
using WaybillService.Application.Files;
using WaybillService.Application.Waybills.Paper.FileImport;
using WaybillService.Application.Waybills.Provider;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Presentation.Controllers.Erros;
using WaybillService.Presentation.Controllers.Waybills.Get;
using WaybillService.Presentation.Controllers.Waybills.GetByOrderId;
using WaybillService.Presentation.Infrastructure;
using WaybillService.Presentation.Infrastructure.Swagger;

namespace WaybillService.Presentation.Controllers.Waybills
{
    [Route("orders/{goodzonOrderId}/waybills")]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [SwaggerOperationFilter(typeof(CompanyIdHeaderParameterOperationFilter))]
    public class WaybillController : ControllerBase
    {
        private readonly IWaybillImporter _waybillImporter;
        private readonly IExcelFileFactory _excelFileFactory;
        private readonly IWaybillProvider _waybillProvider;
        private readonly IWaybillRepository _waybillRepository;

        public WaybillController(
            IWaybillImporter waybillImporter,
            IExcelFileFactory excelFileFactory,
            IWaybillProvider waybillProvider,
            IWaybillRepository waybillRepository)
        {
            _waybillImporter = waybillImporter;
            _excelFileFactory = excelFileFactory;
            _waybillProvider = waybillProvider;
            _waybillRepository = waybillRepository;
        }

        [HttpPost]
        [SwaggerOperationFilter(typeof(MultipleFilesUploadOperationFilter))]
        public async Task<ActionResult> Add(long goodzonOrderId, IFormFileCollection files)
        {
            files = GetCollectionFromArgumentOrRequest(files);

            if (!files.Any())
            {
                throw new BusinessException("No files uploaded");
            }

            var orderId = new OrderId(goodzonOrderId);
            var excelFiles = CreateExcelFiles(files);

            await TransactionRunner.RunWithTransactionAsync(
                () => _waybillImporter.ImportFromFiles(orderId, excelFiles));

            return new OkResult();
        }

        [HttpGet, Route("{supplierWaybillId}")]
        public async Task<ActionResult<WaybillsGetResponse>> Get(long supplierWaybillId)
        {
            var waybillId = new WaybillId(supplierWaybillId);

            var waybill = await _waybillRepository.Get(waybillId);

            return WaybillsGetMapper.MapToResponse(waybill);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<WaybillsGetByOrderIdResponse>>> GetByOrderId(
            long goodzonOrderId)
        {
            var orderId = new OrderId(goodzonOrderId);

            var waybills = await _waybillProvider.Get(orderId);

            var responseViewModel = waybills
                .Select(WaybillsGetByOrderIdMapper.MapToResponse)
                .ToArray();

            return responseViewModel;
        }

        private IFormFileCollection GetCollectionFromArgumentOrRequest(
            IFormFileCollection formFiles)
        {
            if (formFiles.Count > 0)
            {
                return formFiles;
            }

            return Request.Form.Files;
        }

        private List<File> CreateExcelFiles(IFormFileCollection files)
        {
            var excelFiles = new List<File>();

            foreach (var file in files)
            {
                using (var stream = file.OpenReadStream())
                {
                    var excelFile = _excelFileFactory.Create(file.FileName, stream);
                    excelFiles.Add(excelFile);
                }
            }

            return excelFiles;
        }
    }
}