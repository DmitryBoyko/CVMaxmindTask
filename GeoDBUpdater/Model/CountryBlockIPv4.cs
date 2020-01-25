using System;

namespace GeoDBUpdater.Model
{
    /*
        network,geoname_id,registered_country_geoname_id,represented_country_geoname_id,is_anonymous_proxy,is_satellite_provider
1.0.0.0/24,2077456,2077456,,0,0
1.0.1.0/24,1814991,1814991,,0,0
1.0.2.0/23,1814991,1814991,,0,0
1.0.4.0/22,2077456,2077456,,0,0
        */

    /// <summary>
    /// CSV Record to parse line
    /// </summary>
    public class CountryBlockIPv4Record
    {
        public string network { get; set; }
        public string geoname_id { get; set; }
        public string registered_country_geoname_id { get; set; }
        public string represented_country_geoname_id { get; set; }
        public string is_anonymous_proxy { get; set; }
        public string is_satellite_provider { get; set; }

    }

    /// <summary>
    /// Database Table representation class of EF
    /// Just in case to make it working I use string data tpe for properties of this class )))
    /// In fact we have to use manual of the Geo Data Provider to implement apropriate data type fields in DB and in the class
    /// </summary>
    public class CountryBlockIPv4
    {
        public Guid id { get; set; }
        public string network { get; set; }
        public string geoname_id { get; set; }
        public string registered_country_geoname_id { get; set; }
        public string represented_country_geoname_id { get; set; }
        public string is_anonymous_proxy { get; set; }
        public string is_satellite_provider { get; set; }

    }
}
