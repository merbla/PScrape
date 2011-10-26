namespace Merbla.PostCode.Scraper.Models
{
    public class PostCode
    {
        public int Code { get; set; }
        public string Locality { get; set; }
        public string State { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public decimal BoundsNorthEastLatitude { get; set; }
        public decimal BoundsNorthEastLongtitude { get; set; }

        public decimal BoundsSouthWestLatitude { get; set; }
        public decimal BoundsSouthWestLongtitude { get; set; }

    }

 


}