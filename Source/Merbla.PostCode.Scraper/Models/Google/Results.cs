using System.Collections.Generic;

namespace Merbla.PostCode.Scraper.Models.Google
{
    public class Results
    {
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
    }
}