using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WaybillService.Database.Context;
using WaybillService.Presentation.Controllers.Waybills;
using WaybillService.Presentation.Controllers.Waybills.GetByOrderId;
using Xunit;
using Xunit.Abstractions;

namespace WaybillService.IntegrationTests.Waybills
{
    public class WaybillsControllerTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly HttpClient _httpClient;

        private const long OrderId = 181147962005;
        private const string UtdFileName = "УПД.xlsx";
        private readonly MultipartFormDataContent _waybillFile = CreateValidWaybillFile();
        private readonly WaybillContext _context = CreateDbContext();

        public WaybillsControllerTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;

            CreateDbContext();

            _httpClient = TestHttpClientFactory.Create(
                outputHelper,
                _context,
                LogLevel.Information);
        }

        /// <see cref="WaybillController.Add" />
        [Fact]
        public async Task When_waybill_is_added_it_appears_in_base()
        {
            var createResponse = await _httpClient.PostAsync(
                $"/orders/{OrderId}/waybills",
                _waybillFile);
            createResponse.EnsureSuccessStatusCode();

            var lastFile = await _context.Waybills
                .Where(x => x.OrderId == OrderId)
                .OrderByDescending(x => x.File.UploadTime)
                .FirstAsync();

            lastFile.Content.Consignee.Should().Be("ООО ВЕКТОР");
            lastFile.ValidationResult.HasConsigneeError.Should().BeFalse();
        }

        private static MultipartFormDataContent CreateValidWaybillFile()
        {
            var utd = FileLoadingUtilities.ReadFromResource(UtdFileName);
            return FileLoadingUtilities.GetFileContentFromStream(utd, UtdFileName);
        }

        private async Task<WaybillsGetByOrderIdResponse> GetLastResult(HttpResponseMessage response)
        {
            _outputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            var entities = await response.Content
                .ReadAsAsync<IReadOnlyList<WaybillsGetByOrderIdResponse>>();
            return entities.Last();
        }

        private static WaybillContext CreateDbContext()
        {
            var contextOptions =
                new DbContextOptionsBuilder<WaybillContext>()
                    .UseInMemoryDatabase("Waybills")
                    .UseLoggerFactory(new NullLoggerFactory())
                    .Options;
            return new WaybillContext(contextOptions);
        }
    }
}