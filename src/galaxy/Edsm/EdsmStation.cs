using System;

namespace Galaxy
{
    [Serializable]
    public struct EdsmStation
    {
        public int Id { get; set; }
        public long? MarketId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double? DistanceToArrival { get; set; }
        public string Allegiance { get; set; }
        public string Government { get; set; }
        public string Economy { get; set; }
        public string SecondEconomy { get; set; }
        public bool? HaveMarket { get; set; }
        public bool? HaveShipyard { get; set; }
        public bool? HaveOutfitting { get; set; }
        public string[] OtherServices { get; set; }
        public UpdateTime UpdateTime { get; set; }
        public int SystemId { get; set; }
        public long SystemId64 { get; set; }
        public string SystemName { get; set; }
    }
}
