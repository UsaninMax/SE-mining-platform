namespace TradePlatform.Commons.Securities
{
    class FinamSecurity : ISecurity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Market { get; set; }
        public string MarketId { get; set; }
        public string Decp { get; set; }
        public string EmitentChild { get; set; }
        public string Url { get; set; }
    }
}
