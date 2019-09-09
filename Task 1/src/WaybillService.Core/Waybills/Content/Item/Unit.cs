namespace WaybillService.Core.Waybills.Content.Item
{
    public class Unit
    {
        public string Name { get; }
        public string OkeiCode { get; }

        public Unit(string name, string okeiCode)
        {
            OkeiCode = okeiCode;
            Name = name;
        }
    }
}