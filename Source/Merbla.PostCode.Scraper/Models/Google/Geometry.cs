namespace Merbla.PostCode.Scraper.Models.Google
{
    public class Geometry
    {
        public string LocationType { get; set; }
        public Location Location { get; set; }
        public Bounds Bounds { get; set; }
    }
}