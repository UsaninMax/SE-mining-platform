namespace TradePlatform.Commons.Securities
{
    public class Market
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var item = obj as Market;

            if (item == null)
            {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}," +
                   $" {nameof(Id)}: {Id}";
        }
    }
}
