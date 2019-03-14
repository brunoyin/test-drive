using System;
using System.Collections.Generic;

namespace CmdlineApp
{
    public partial class College
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int? Region { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? AdmRate { get; set; }
        public decimal? SatAvg { get; set; }
        public decimal? ActAvg { get; set; }
        public decimal? Earnings { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Enrollments { get; set; }

        public string Info {
            get{
                return $"{this.Id}: {this.Name}, costs ${this.Cost:#,##0}, average earnings in 10 years ${this.Earnings:#,##0}";
        }}
    }
}
