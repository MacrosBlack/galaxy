using System;

namespace Galaxy
{
    [Serializable]
    public struct EdsmSystem
    {
        public int Id { get; set; }
        public Int64? Id64 { get; set; }
        public string Name { get; set; }
        public Coords? Coords { get; set; }
        public DateTime? Date { get; set; }
    }

    [Serializable]
    public struct Coords
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
