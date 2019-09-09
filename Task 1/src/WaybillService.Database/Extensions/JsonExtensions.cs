using Newtonsoft.Json;

namespace WaybillService.Database.Extensions
{
    public static class JsonExtensions
    {
        public static T[] GetArrayFromJsonOrEmptyArray<T>(string jsonString) =>
            string.IsNullOrWhiteSpace(jsonString)
                ? new T[0]
                : JsonConvert.DeserializeObject<T[]>(jsonString);

        public static string SetArrayJsonStringOrEmptyArrayString<T>(T[] array) =>
            JsonConvert.SerializeObject(array ?? new T[0]);
    }
}