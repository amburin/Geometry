using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace WaybillService.IntegrationTests
{
    public static class FileLoadingUtilities
    {
        public static async Task WriteToDesktopFile(Stream stream, string fileName)
        {
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                fileName);

            using (var fileStream = new FileStream(
                filePath,
                FileMode.OpenOrCreate,
                FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        
        public static Stream ReadFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var fullResourceName = assembly.GetManifestResourceNames()
                .Single(x => x.EndsWith(resourceName));

            return assembly.GetManifestResourceStream(fullResourceName);
        }
        
        internal static MultipartFormDataContent GetFileContentFromStream(Stream fileData,
            string fileName)
        {
            var boundary = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
            var content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type",
                "multipart/form-data; boundary=" + boundary);
            content.Add(new StreamContent(fileData), fileName, fileName);
            return content;
        }
        
        public static Task<string> ReadJsonFromResource(string resourceName)
        {
            using (var stream = ReadFromResource(resourceName))
            {
                using (var stringReader = new StreamReader(stream))
                {
                    return stringReader.ReadToEndAsync();
                }
            }
        }
    }
}