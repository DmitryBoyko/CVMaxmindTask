using System;

namespace GeoLocator.Entities
{
    
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
