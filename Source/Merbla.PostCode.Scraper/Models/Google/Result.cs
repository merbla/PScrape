#region Using Directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Merbla.PostCode.Scraper.Models.Google
{
    public class Result
    {
        public List<Results> Results { get; set; }

        public string Status { get; set; }

        public decimal Longitude
        {
            get
            {
                if (Results.Any())
                {
                    return Results.First().Geometry.Location.Lng;
                }
                return new decimal();
            }
        }

        public decimal Latitude
        {
            get
            {
                if(Results.Any())
                {
                    return Results.First().Geometry.Location.Lat;
                }
                return new decimal();
            }
        }
    }
}