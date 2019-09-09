using System;
using System.Globalization;
using Newtonsoft.Json;

namespace WaybillService.Core.Files
{
    [JsonConverter(typeof(FileIdJsonConverter))]
    public struct CephFileId : IEquatable<CephFileId>
    {
        private readonly Guid _value;
        
        public CephFileId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"Invalid value. FileId must not be empty.");
            }

            _value = value;
        }

        public static bool operator !=(
            CephFileId value1,
            CephFileId value2) =>
            !value1.Equals(value2);

        public static bool operator ==(
            CephFileId value1,
            CephFileId value2) =>
            value1.Equals(value2);

        public bool Equals(CephFileId other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CephFileId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString() =>
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            _value.ToString("D", CultureInfo.InvariantCulture);

        public Guid ToGuid() => _value;
    }

    public class FileIdJsonConverter : JsonConverter<CephFileId>
    {
        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, CephFileId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToGuid());
        }

        public override CephFileId ReadJson(
            JsonReader reader,
            Type objectType,
            CephFileId existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            return new CephFileId(Guid.Parse(reader.Value.ToString()));
        }
    }
}