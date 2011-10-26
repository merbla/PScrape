using System.Diagnostics;
using CsvHelper.Configuration;

namespace Merbla.PostCode.Scraper.Maps
{
    public class ReadPostCodeMap : CsvClassMap<Models.PostCode>
    {
        public ReadPostCodeMap()
        {
            Map(x => x.Code).Name("Pcode");
           // Map(x => x.Locality).Name("Locality");
            Map(x => x.State).Name("State");

            Map(x => x.Latitude).Ignore();
            Map(x => x.Longitude).Ignore();

            Map(x => x.BoundsNorthEastLatitude).Ignore();
            Map(x => x.BoundsNorthEastLongtitude).Ignore();
            Map(x => x.BoundsSouthWestLatitude).Ignore();
            Map(x => x.BoundsSouthWestLongtitude).Ignore();
        }
    }
}