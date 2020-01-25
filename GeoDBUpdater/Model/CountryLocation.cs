using System;

namespace GeoDBUpdater.Model
{
    /*
     geoname_id,locale_code,continent_code,continent_name,country_iso_code,country_name,is_in_european_union
49518,zh-CN,AF,"非洲",RW,"卢旺达",0
51537,zh-CN,AF,"非洲",SO,"索马里",0
69543,zh-CN,AS,"亚洲",YE,"也门",0
99237,zh-CN,AS,"亚洲",IQ,"伊拉克",0
         */

    /// <summary>
    /// CSV Record to parse line
    /// </summary>
    public class CountryLocationRecord
    {
        public string geoname_id { get; set; }

        public string locale_code { get; set; }

        public string continent_code { get; set; }

        public string continent_name { get; set; }

        public string country_iso_code { get; set; }

        public string country_name { get; set; }

        public string is_in_european_union { get; set; }

    }

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
