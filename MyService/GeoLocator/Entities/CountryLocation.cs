using System;

namespace GeoLocator.Entities
{
 

    /// <summary>
    /// Database Table representation class of EF
    /// Just in case to make it working I use string data tpe for properties of this class )))
    /// In fact we have to use manual of the Geo Data Provider to implement apropriate data type fields in DB and in the class
    /// </summary>
    public class CountryLocation
    {
        public Guid id { get; set; }
        public string geoname_id { get; set; }

        public string locale_code { get; set; }

        public string continent_code { get; set; }

        public string continent_name { get; set; }

        public string country_iso_code { get; set; }

        public string country_name { get; set; }

        public string is_in_european_union { get; set; }

    }
}
