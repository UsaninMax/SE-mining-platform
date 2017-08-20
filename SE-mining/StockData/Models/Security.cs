namespace SEMining.StockData.Models
{
    public class Security
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Market Market { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}," +
                   $" {nameof(Name)}: {Name}," +
                   $" {nameof(Code)}: {Code}," +
                   $" {nameof(Market)}: {Market}";
        }
    }
}
