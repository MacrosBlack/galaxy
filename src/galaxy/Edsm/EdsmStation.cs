using System;
using System.Linq;

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

        // From OtherServices
        public bool Restock { get; set; }
        public bool Refuel { get; set; }
        public bool Repair { get; set; }
        public bool Contacts { get; set; }
        public bool UniversalCartographics { get; set; }
        public bool Missions { get; set; }
        public bool CrewLounge { get; set; }
        public bool Tuning { get; set; }
        public bool SearchandRescue { get; set; }
        public bool BlackMarket { get; set; }
        public bool InterstellarFactorsContact { get; set; }
        public bool MaterialTrader { get; set; }
        public bool TechnologyBroker { get; set; }

        public void SetOtherServices()
        {
            if (OtherServices.Length == 0)
            {
                return;
            }

            if (OtherServices.Any(p => p == "Restock"))
            {
                Restock = true;
            }
            if (OtherServices.Any(p => p == "Refuel"))
            {
                Refuel = true;
            }
            if (OtherServices.Any(p => p == "Repair"))
            {
                Repair = true;
            }
            if (OtherServices.Any(p => p == "Contacts"))
            {
                Contacts = true;
            }
            if (OtherServices.Any(p => p == "Universal Cartographics"))
            {
                UniversalCartographics = true;
            }
            if (OtherServices.Any(p => p == "Missions"))
            {
                Missions = true;
            }
            if (OtherServices.Any(p => p == "Crew Lounge"))
            {
                CrewLounge = true;
            }
            if (OtherServices.Any(p => p == "Tuning"))
            {
                Tuning = true;
            }
            if (OtherServices.Any(p => p == "Search and Rescue"))
            {
                SearchandRescue = true;
            }
            if (OtherServices.Any(p => p == "Black Market"))
            {
                BlackMarket = true;
            }
            if (OtherServices.Any(p => p == "Interstellar Factors Contact"))
            {
                InterstellarFactorsContact = true;
            }
            if (OtherServices.Any(p => p == "Material Trader"))
            {
                MaterialTrader = true;
            }
            if (OtherServices.Any(p => p == "TechnologyBroker"))
            {
                TechnologyBroker = true;
            }
        }
    }
}
