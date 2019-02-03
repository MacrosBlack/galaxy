using System;

namespace Galaxy
{
    [Serializable]
    public class EdsmBody
    {
        public int Id { get; set; }
        public long? Id64 { get; set; }
        public int? BodyId { get; set; }
        public string Name { get; set; }
        public EdsmDiscovery Discovery { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public double? Offset { get; set; }
        public EdsmParents[] Parents { get; set; }
        public double? DistanceToArrival { get; set; }
        public bool? IsMainstar { get; set; }
        public bool? IsScoopable { get; set; }
        public string SpectralClass { get; set; }
        public string Luminosity { get; set; }
        public double? AbsoluteMagnitude { get; set; }
        public double? SolarMasses { get; set; }
        public double? SolarRadius { get; set; }
        public double? SurfaceTemperature { get; set; }
        public double? OrbitalPeriod { get; set; }
        public double? SemiMajorAxis { get; set; }
        public double? OrbitalExcentricity { get; set; }
        public double? OrbitalInclination { get; set; }
        public double? ArgOfPeriapsis { get; set; }
        public double? RotationalPeriod { get; set; }
        public bool? rotationalPeriodTidallyLocked { get; set; }
        public double? AxialTilt { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int SystemId { get; set; }
        public long? SystemId64 { get; set; }
        public string SystemName { get; set; }
    }
}
