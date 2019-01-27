using System;

namespace Galaxy
{
    [Serializable]
    public class UpdateTime
    {
        public DateTime Information { get; set; }
        public string Market { get; set; }
        public DateTime? Shipyard { get; set; }
        public DateTime? Outfitting { get; set; }
    }
}