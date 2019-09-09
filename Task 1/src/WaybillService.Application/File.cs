using System.Collections.ObjectModel;

namespace WaybillService.Application
{
    public abstract class File
    {
        protected File(string name, byte[] bytes)
        {
            Name = name;
            Bytes = new ReadOnlyCollection<byte>(bytes);
        }

        public string Name { get; }
        public abstract string ContentType { get; }
        public ReadOnlyCollection<byte> Bytes { get; }
    }
}