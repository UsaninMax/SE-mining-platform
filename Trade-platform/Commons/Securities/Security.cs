﻿namespace TradePlatform.Commons.Securities
{
    public class Security
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Market Market { get; set; }
        public string Decp { get; set; }
        public string EmitentChild { get; set; }
        public string Url { get; set; }
    }
}