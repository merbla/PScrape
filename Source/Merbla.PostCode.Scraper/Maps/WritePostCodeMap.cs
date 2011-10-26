using CsvHelper.Configuration;

namespace Merbla.PostCode.Scraper.Maps
{
    public class WritePostCodeMap : CsvClassMap<Models.PostCode>
    {
        public WritePostCodeMap()
        {
            Map(x => x.Code).Name("Postcode");
            Map(x => x.State);

            Map(x => x.Latitude);
            Map(x => x.Longitude);

            Map(x => x.BoundsNorthEastLatitude);
            Map(x => x.BoundsNorthEastLongtitude);
            Map(x => x.BoundsSouthWestLatitude);
            Map(x => x.BoundsSouthWestLongtitude);
        }
    }
}