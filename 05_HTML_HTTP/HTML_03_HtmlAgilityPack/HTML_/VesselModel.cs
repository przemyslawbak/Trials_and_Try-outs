using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_
{
    public class VesselModel
    {
        public int VesselId { get; set; } //
        public int? IMO { get; set; } //repo
        public DateTime? DatatUpdate { get; set; } //repo
        public string Name { get; set; } //repo
        public int? MMSI { get; set; } //repo
        public string CallSign { get; set; } //vf/
        public int? GRT { get; set; } //vf/
        public int? DWT { get; set; } //vf/
        public string VesselType { get; set; } //
        public DateTime? VesselTypeSince { get; set; } //-no display
        public int? YOB { get; set; } //
        public string Shipbuilder { get; set; } //
        public string VesselStatus { get; set; } //
        public string Classification { get; set; } //
        public string Flag { get; set; } //vf
        public string PicturesGalleryLink { get; set; } //-no display
        public string[] PreviousOwners { get; set; } //-no display
        public string[] PreviousManagers { get; set; } //-no display
        //performances
        public DateTime? PerformancesUpdate { get; set; } //-no display
        public string MapLink { get; set; } //-no display
        public double? LOA { get; set; } //vf
        public double? Breadth { get; set; } //vf
        public double? SpeedMax { get; set; } //repo/
        public double? SpeedAverage { get; set; } //repo/
        //voyage data
        public DateTime? AISLatestActivity { get; set; } //vf
        public double? Lat { get; set; } //vf
        public double? Lon { get; set; } //vf
        public string GeographicalArea { get; set; } //repo/
        public string AISStatus { get; set; } //vt
        public double? Speed { get; set; } //vf
        public double? Course { get; set; } //vf
        public string Destination { get; set; } //vf
        public double? Draught { get; set; } //vf
        public DateTime? ETA { get; set; } //vf
        public int? OwnerId { get; set; } //repo/
        public int? ManagerId { get; set; } //repo/
        public string[] DetailedType { get; set; } //vf/sp
        public string AisVesselType { get; set; } //vf
        public string PortCurrent { get; set; } //-no display
        public string PortNext { get; set; } //-no display
    }
}
